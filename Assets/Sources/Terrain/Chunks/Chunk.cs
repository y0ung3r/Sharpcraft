using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ChunkRenderer))]
public class Chunk : MonoBehaviour, IEnumerable<Block>
{
	private IDictionary<Vector3Int, Block> _blocks;

	public const int Width = 16;

	public const int MaxHeight = 256;

	public const int Depth = 16;

	public Terrain Terrain 
		=> GetComponentInParent<Terrain>();

	public Vector2Int WorldPosition 
		=> Vector2Int.FloorToInt(new Vector2(transform.position.x, transform.position.z));

	public int TotalHeight
		=> _blocks.Values.Max(block => block.LocalPosition.y);

	public void Awake()
	{
		_blocks = new Dictionary<Vector3Int, Block>();
	}

	public void Start()
	{
		GetComponent<ChunkRenderer>()
			.Render(this);
	}

	public Block AddBlock(BlockType type, Vector3Int localPosition)
	{
		if (TryAddBlock(type, localPosition, out var block))
		{
            return block;
        }

        throw new OperationCanceledException();
	}

	public IEnumerable<Block> GetOfType(BlockType type)
		=> _blocks.Where(pair => pair.Value.Type == type)
				.Select(pair => pair.Value);

	public bool TryAddBlock(BlockType type, Vector3Int localPosition, out Block block)
	{
        block = new Block(this, type, localPosition);
		return _blocks.TryAdd(localPosition, block);
    }

    public bool TryGetBlock(Vector3Int localPosition, out Block block)
		=> _blocks.TryGetValue(localPosition, out block);

    public bool TryRemoveBlock(Vector3Int localPosition)
		=> _blocks.Remove(localPosition);

	public bool HasBlock(Vector3Int localPosition)
		=> TryGetBlock(localPosition, out _);

    public IEnumerator<Block> GetEnumerator()
		=> _blocks.Select(pair => pair.Value)
			.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator()
		=> GetEnumerator();
}