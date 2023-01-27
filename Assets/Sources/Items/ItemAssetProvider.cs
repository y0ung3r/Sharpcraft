using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Provider")]
public class ItemAssetProvider : ScriptableObject
{
	[SerializeField]
	private ItemAsset[] _assets;

	public ItemAsset GetAsset(ItemType type)
	{
		var asset = _assets.FirstOrDefault(itemAsset => itemAsset.Type == type);

		if (asset is null)
		{
			throw new InvalidOperationException("Эскиз предмета не найден");
		}

		return asset;
	}
}