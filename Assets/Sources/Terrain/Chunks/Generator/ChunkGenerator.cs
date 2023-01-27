using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkGenerator : MonoBehaviour
{
	[SerializeField] 
	private Chunk _chunkTemplate;
	
	[SerializeField]
	private NoiseGenerator _noiseGenerator;

	public Chunk Generate(Vector2Int position)
	{
		var chunk = Instantiate
		(
			_chunkTemplate, 
			new Vector3(position.x, 0, position.y), 
			Quaternion.identity, 
			transform
		);

		for (var x = 0; x < Chunk.Width; x++)
		{
			for (var z = 0; z < Chunk.Depth; z++)
			{
				var noise = _noiseGenerator.GetNoise(x + position.x, z + position.y);
				var height = Convert.ToInt32(noise);

				for (var y = 0; y < height; y++)
				{
					if (y == 0)
					{
						chunk.AddBlock(BlockType.Bedrock, new Vector3Int(x, y, z));
					}

					if (y > 0 && y < height - 6)
					{
						chunk.AddBlock(BlockType.Stone, new Vector3Int(x, y, z));
					}

                    if (y >= height - 6 && y < height - 2)
                    {
                        chunk.AddBlock(BlockType.Dirt, new Vector3Int(x, y, z));
                    }

                    if (y >= height - 2 && y < height - 1)
					{
						chunk.AddBlock(BlockType.Grass, new Vector3Int(x, y, z));
					}
                }
            }
		}

		var grassBlocks = chunk.GetOfType(BlockType.Grass)
			.Select(block => block.LocalPosition)
			.OrderBy(_ => Guid.NewGuid())
			.Take(25);

		foreach (var grassBlock in grassBlocks)
		{
			var weedPosition = new Vector3Int(grassBlock.x, grassBlock.y + 1, grassBlock.z);
			chunk.TryAddBlock(BlockType.Weed, weedPosition, out _);
		}

        return chunk;
	}
}