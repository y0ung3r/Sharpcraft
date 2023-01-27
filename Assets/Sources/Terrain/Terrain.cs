using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ChunkGenerator))]
public class Terrain : MonoBehaviour, IEnumerable<Chunk>
{
	private IDictionary<Vector2Int, Chunk> _chunks;

	private ChunkGenerator _chunkGenerator;

	[SerializeField]
	private Player _player;

	[SerializeField]
	private ItemFactory _itemFactory;
	
	[SerializeField] 
	private int _terrainWidth = 2;

	[SerializeField] 
	private int _terrainHeight = 2;

	public Player Player
		=> _player;

	public ItemFactory ItemFactory
		=> _itemFactory;

    public void OnEnable()
    {
		Player.Inventory
			.ItemDropped += OnInventoryItemDropped;
    }

    public void OnDisable()
    {
        Player.Inventory
            .ItemDropped -= OnInventoryItemDropped;
    }

    private void OnInventoryItemDropped(ItemType itemType, int amount)
    {
		for (var counter = 0; counter < amount; counter++)
		{
			var droppedItem = ItemFactory.Create(itemType)
				.GetComponent<PhysicalItem>();

			Player.PushItemAway(droppedItem);
		}
    }

    public void Awake()
	{
		_chunks = new Dictionary<Vector2Int, Chunk>();
		_chunkGenerator = GetComponent<ChunkGenerator>();

		var terrainSize = new Size(_terrainWidth, _terrainHeight);
		Seed(terrainSize);
	}

	public Chunk AddChunk(Vector2Int position)
	{
		var chunk = _chunkGenerator.Generate(position);
		
		if (_chunks.TryAdd(position, chunk))
		{
			return chunk;
		}

        throw new OperationCanceledException();
    }

	public bool TryGetBlock(Vector3Int worldPosition, out Block block)
	{
		block = default;

        var chunkOrigin = worldPosition.GetOrigin(Chunk.Width, Chunk.Depth);
        var localPosition = worldPosition.ToLocal(chunkOrigin);

		if (!TryGetChunk(chunkOrigin, out var chunk))
		{
            return false;
        }

        return chunk.TryGetBlock(localPosition, out block);
    }

	public bool HasBlock(Vector3Int worldPosition)
		=> TryGetBlock(worldPosition, out _);

	public bool TryGetChunk(Vector2Int origin, out Chunk chunk)
		=> _chunks.TryGetValue(origin, out chunk);

    private void Seed(Size size)
	{
		var halfWidth = size.Width / 2;
		var halfHeight = size.Height / 2;
		
		for (var x = -halfWidth; x < halfWidth; x++)
		{
			for (var y = -halfHeight; y < halfHeight; y++)
			{
				var chunkPosition = new Vector2Int(x * Chunk.Width, y * Chunk.Depth);
				AddChunk(chunkPosition);
			}
		}
	}
	
	public IEnumerator<Chunk> GetEnumerator()
	{
		return _chunks.Select(pair => pair.Value)
			.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}