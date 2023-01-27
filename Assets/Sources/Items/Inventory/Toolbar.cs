using UnityEngine;

public class Toolbar : ItemHolderContainer
{
	[SerializeField]
	private ItemHolderContainer _outerContainer;

	[SerializeField]
	private ItemHolderSelection _selectionTemplate;

	private int _selectedHolderIndex;

	public ItemHolder SelectedHolder
		=> _outerContainer.GetHolder(_selectedHolderIndex);

	public ItemHolderSelection CurrentSelection
		=> SelectedHolder.GetComponentInChildren<ItemHolderSelection>();

	public void Select(int index)
	{
		if (index == -1 && CurrentSelection != null)
		{
			CurrentSelection.Release();
			return;
		}
		
		var targetHolder = _outerContainer.GetHolder(index);
		
		// ReSharper disable once Unity.NoNullCoalescing
		var selection = CurrentSelection ?? Instantiate(_selectionTemplate);
		selection.Select(targetHolder);
		
		_selectedHolderIndex = index;
	}

	public void Deselect()
		=> Select(index: -1);

	protected override void OnItemPlaced(int holderIndex, Item item, int amount)
	{
		var outerItem = Inventory.ItemFactory
			.Create(item.Type);

		_outerContainer.GetHolder(holderIndex)
			.PutItem(outerItem, amount);
	}

	protected override void OnItemDisplaced(int holderIndex, Item item)
	{
		var outerSlot = _outerContainer.GetHolder(holderIndex);

		if (!outerSlot.HasItem)
		{
			return;
		}

		outerSlot.Release();
	}
}