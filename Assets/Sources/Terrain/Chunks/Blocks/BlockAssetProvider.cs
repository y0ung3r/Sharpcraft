using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Blocks/Provider")]
public class BlockAssetProvider : ScriptableObject
{
	[SerializeField] 
	private BlockAsset[] _assets;

	public BlockAsset GetAsset(Block block)
	{
		var asset = _assets.FirstOrDefault(blockAsset => blockAsset.Type == block.Type);

		if (asset == null)
		{
			throw new InvalidOperationException();
		}

		asset.AssetProvider = this;

		return asset;
	}
}