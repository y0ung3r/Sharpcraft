using UnityEngine;

[CreateAssetMenu(menuName = "Items/Views/ItemFactory")]
public class ItemFactory : ScriptableObject
{
	[SerializeField]
	private Item _itemPrefab;

	[SerializeField] 
	private ItemAssetProvider _assetProvider;

	public Item Create(ItemType type)
	{
		var asset = _assetProvider.GetAsset(type);
		
		var view = Instantiate(_itemPrefab);
		view.name = type.ToString();
		view.Render(asset);
		
		return view;
	}
}