using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	private Camera _camera;

	private Vector3 _rotation;

	[SerializeField]
	[Range(1.0f, 20.0f)]
	private float _mouseSensivity = 2.1f;

	[SerializeField]
	private float _viewPointMaxDistance = 6.0f;

	private float _yaw;

	private float _pitch;

	public const float MinVerticalRotationAngle = -90.0f;
	
	public const float MaxVerticalRotationAngle = 90.0f;

    public Vector3 Rotation
	{
		get => _rotation;
		private set => transform.eulerAngles = _rotation = value;
    }

	public Vector3 Direction
		=> Vector3.Normalize(transform.forward);

    private void Awake()
		=> _camera = GetComponent<Camera>();

	public Vector3Int? GetViewPoint(bool inside)
	{
        var screenCenter = new Vector3(0.5f, 0.5f);
        var ray = _camera.ViewportPointToRay(screenCenter);

        if (!Physics.Raycast(ray, out var hit, _viewPointMaxDistance))
        {
            return null;
        }

		var insideOffset = inside ? -1.0f : 1.0f;
        var blockCenter = hit.point + hit.normal / 2.0f * insideOffset;
        return Vector3Int.FloorToInt(blockCenter);
    }

    public void ContinueRotate()
	{
        _yaw += _mouseSensivity * Input.GetAxis("Mouse X");
        _pitch -= _mouseSensivity * Input.GetAxis("Mouse Y");

        Rotation = new Vector3
		(
			x: Mathf.Clamp(_pitch, MinVerticalRotationAngle, MaxVerticalRotationAngle), 
			y: _yaw,
			z: 0.0f
		);
    }
}