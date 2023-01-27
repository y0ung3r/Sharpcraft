using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BlockAsset : ScriptableObject
{
	[SerializeField] 
	private Material _material;
	
	[SerializeField] 
	private BlockType _blockType;

	[SerializeField]
	private BlockTransparency _transparency;

	public Material Material 
		=> _material;
	
	public BlockType Type 
		=> _blockType;

	public BlockTransparency Transparency
		=> _transparency;

	public BlockAssetProvider AssetProvider { get; set; }

    private bool IsVisibleSide(BlockSide side, Block block)
    {
        if (!block.TryGetAdjacentBlock(side.Direction, out var adjacentBlock))
        {
            return true;
        }

        var adjacentBlockAsset = AssetProvider
            .GetAsset(adjacentBlock);

        return adjacentBlockAsset.Transparency == BlockTransparency.Transparent;
    }

    public virtual IEnumerable<BlockSide> GetSides(Block block)
	{
		var sides = new BlockSide[6]
		{
			new LeftBlockSide(),
			new RightBlockSide(),
			new FrontBlockSide(),
			new BackBlockSide(),
			new UpperBlockSide(),
			new LowerBlockSide()
		};

		return sides.Where(side => IsVisibleSide(side, block))
			.ToList();
    }

	public abstract IEnumerable<Vector2> GetUVs(BlockSide side);
}