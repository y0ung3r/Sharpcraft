using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _rotation;

    [SerializeField]
    private PlayerCamera _playerCamera;

    [SerializeField]
    private Terrain _terrain;

    [SerializeField]
    private Inventory _inventory;

	[Header("Movement")]
	[SerializeField]
    private float _speed = 2.0f;

    [SerializeField]
    private bool _isOnGround;

    [SerializeField]
    private float _jumpHeight = 1.0f;

    [SerializeField]
    private float _gravity = -9.81f;

    private CharacterController _controller;

    private Vector3 _velocity;

    [SerializeField]
    private KeyCode _jumpKey = KeyCode.Space;

    [Header("Terrain Detection")]
    [SerializeField]
    private float _distance = 0.2f;

    [SerializeField]
    private Transform _legs;

    [SerializeField]
    private LayerMask _terrainMask;

    [Header("Item Interactions")]
    [SerializeField]
	private float _itemPushForce = 6.0f;

    public PlayerCamera Camera 
		=> _playerCamera;

	public Inventory Inventory 
		=> _inventory;

	public Vector3 Rotation
	{
		get => _rotation;
		private set => transform.localEulerAngles = _rotation = value;
	}

	public Vector3 Position
	{
		get => transform.position;
		set => transform.position = value;
	}

	public bool FreezeRotations { get; set; }

    public bool StopBuilding { get; set; }

    public bool StopBreaking { get; set; }

    public void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void Update()
	{
		ContinueRotate();
        ContinueMove();

        if (Input.GetMouseButtonDown(0))
        {
            Break();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Build();
        }
    }

    private void Build()
    {
        if (StopBuilding)
        {
            return;
        }

        var cameraViewPoint = _playerCamera.GetViewPoint(inside: false);

        if (!cameraViewPoint.HasValue)
        {
            return;
        }

        var worldPosition = cameraViewPoint
            .Value;

        var chunkOrigin = worldPosition
            .GetOrigin(Chunk.Width, Chunk.Depth);

        if (!_terrain.TryGetChunk(chunkOrigin, out var chunk))
        {
            return;
        }

        var localPosition = worldPosition.ToLocal(chunkOrigin);
        var selectedToolbarHolder = Inventory.Toolbar.SelectedHolder;

        if (selectedToolbarHolder.HasItem)
        {
            var blockType = selectedToolbarHolder.Item.Type.ToBlockType();

            if (blockType.HasValue && chunk.TryAddBlock(blockType.Value, localPosition, out var _))
            {
                selectedToolbarHolder.UseItem();

                chunk.GetComponent<ChunkRenderer>()
                    .Render(chunk);
            }
        }
    }

    private void Break()
    {
        if (StopBreaking)
        {
            return;
        }

        var cameraViewPoint = _playerCamera.GetViewPoint(inside: true);

        if (!cameraViewPoint.HasValue)
        {
            return;
        }

        var worldPosition = cameraViewPoint
            .Value;

        var chunkOrigin = worldPosition
            .GetOrigin(Chunk.Width, Chunk.Depth);

        if (!_terrain.TryGetChunk(chunkOrigin, out var chunk))
        {
            return;
        }

        var localPosition = worldPosition.ToLocal(chunkOrigin);

        if (chunk.TryRemoveBlock(localPosition))
        {
            chunk.GetComponent<ChunkRenderer>()
                .Render(chunk);
        }
    }

	private void ContinueRotate()
	{
        if (FreezeRotations)
        {
            return;
        }

		Camera.ContinueRotate();
        Rotation = new Vector3(0.0f, Camera.Rotation.y, 0.0f);
    }

	private void ContinueMove()
	{
        _isOnGround = Physics.CheckSphere(_legs.position, _distance, _terrainMask);

        if (_isOnGround && _velocity.y < 0)
        {
            _velocity.y = 0f;
        }

        var horizontalAxis = Input.GetAxis("Horizontal");
        var verticalAxis = Input.GetAxis("Vertical");
        var direction = transform.forward * verticalAxis + transform.right * horizontalAxis;
        _controller.Move(direction * Time.deltaTime * _speed);

        if (Input.GetKeyDown(_jumpKey) && _isOnGround)
        {
            _velocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);
        }

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    public void PushItemAway(PhysicalItem item)
	{
        item.SetPosition(Position + Camera.Direction);
        item.Push(Camera.Direction * _itemPushForce);
    }

    private void OnTriggerEnter(Collider other)
    {
		if (Inventory.IsOpen)
		{
			return;
		}

        if (other.transform.parent.TryGetComponent<PhysicalItem>(out var item))
		{
			var predicate = new Func<ItemHolder, bool>(
				holder => !holder.HasItem || (holder.Item.Stackable && holder.Item.Type == item.Type));

			var freeHolder = Inventory.Toolbar
				.GetFirstHolder(predicate) ?? Inventory.Backpack
					.GetFirstHolder(predicate);

			var inventoryItem = Inventory.ItemFactory
				.Create(item.Type);

			freeHolder.PutItem(inventoryItem);

			item.Release();
		}
    }
}