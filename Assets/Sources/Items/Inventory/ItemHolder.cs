using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ItemHolder : MonoBehaviour, IPointerClickHandler, ICleanup
{
    [SerializeField]
    private AmountHolder _amountHolder;

    public event Action<int, Item, int> ItemPlaced;

    public event Action<ItemType, int> ItemDropped;

    public event Action<int, Item> Cleaned;

    public event Action Released;

    public Inventory Inventory
    {
        get
        {
            if (OwningContainer != null)
            {
                // Более производительный способ получить ссылку на инвентарь
                return OwningContainer.Inventory;
            }

            return GetComponentInParent<Inventory>();
        }
    }

    public ItemHolderContainer OwningContainer
		=> GetComponentInParent<ItemHolderContainer>(includeInactive: true);

	public Item Item
	{
		get => GetComponentInChildren<Item>();

		set
        {
            if (value is IAttachable attachable)
            {
                attachable.AttachTo(this);
            }
        }
    }

	public bool HasItem
		=> Item != null;

    public AmountHolder AmountHolder
        => _amountHolder;

    public int Index
    {
        get
        {
            if (OwningContainer == null)
            {
                return -1;
            }

            return OwningContainer.IndexOf(this);
        }
    }

    public bool HasItemsStack
        => HasItem && AmountHolder.HasStack;

    public virtual void PutItem(Item item, int amount)
    {
        if (!CanBePlaced(item))
        {
            throw new OccupiedHolderException();
        }

        if (!item.Stackable)
        {
            for (var nextHolderIndex = Index + 1; nextHolderIndex < Index + amount; nextHolderIndex++)
            {
                var nextFreeHolder = OwningContainer
                    .GetFirstFreeHolder(nextHolderIndex);

                if (nextFreeHolder != null)
                {
                    var nextItem = Inventory.ItemFactory
                        .Create(item.Type);

                    nextFreeHolder
                        .PutItem(nextItem);
                }
            }

            amount = 1;
        }

        if (!HasItem)
        {
            Item = item;
        }
        else if (item is ICleanup cleanup)
        {
            cleanup.Release();
        }

        AmountHolder.Amount += amount;
        ItemPlaced?.Invoke(Index, Item, amount);
    }

    public void PutItem(Item item)
        => PutItem(item, amount: 1);

    public void MoveItemTo(ItemHolder targetHolder)
    {
        var item = Item;
        var amount = AmountHolder.Amount;

        Clear();

        targetHolder.PutItem(item, amount);
    }

    public bool CanBePlaced(Item item)
        => item != null && (!HasItem || (Item.Stackable && Item.Type == item.Type));

    public void OnPointerClick(PointerEventData eventData)
	{
        if (!TryGetComponent<ItemHolder>(out var currentHolder))
        {
            return;
        }

        if (HasItem && !Inventory.HasDraggableHolder)
        {
            Inventory.BeginDragHolder(currentHolder);

            return;
        }

        var currentDragging = Inventory.CurrentDragging;

        if (currentDragging == null)
        {
            return;
        }

		var draggableItem = currentDragging
            .DraggableHolder?
            .Item;

        if (!CanBePlaced(draggableItem))
        {
            currentHolder.MoveItemTo(currentDragging.SourceHolder);

            currentDragging.DraggableHolder
                .MoveItemTo(currentHolder);

            Inventory.BeginDragHolder(currentDragging.SourceHolder);

            return;
        }

		switch (eventData.button)
		{
			case PointerEventData.InputButton.Left:
                OnItemStackIncoming(currentDragging.DraggableHolder);
                break;

			case PointerEventData.InputButton.Right:
                OnItemIncoming(draggableItem);
                break;

			default:
				return;
        }

		if (!currentDragging.DraggableHolder.HasItem)
		{
            Inventory.EndDragHolder();
		}
    }

    public void DropItem(int amount)
    {
        if (AmountHolder.Amount < amount)
        {
            throw new ItemDropException(amount);
        }

        var itemType = Item.Type;

        if (!HasItemsStack || AmountHolder.Amount - amount == 0)
        {
            Release();
        }

        AmountHolder.Amount -= amount;
        ItemDropped?.Invoke(itemType, amount);
    }

    public void UseItem()
    {
        if (!HasItemsStack || AmountHolder.Amount - 1 == 0)
        {
            Release();
        }

        AmountHolder.Amount--;
    }

    public void DropItem()
        => DropItem(amount: 1);

    public void Clear()
    {
        if (!HasItem)
        {
            return;
        }

        var item = Item;

        if (Item is IAttachable attachable)
        {
            attachable.Detach();
        }

        AmountHolder.Amount = 0;

        Cleaned?.Invoke(Index, item);
    }

    public void Release()
    {
        if (!HasItem)
        {
            return;
        }

        var item = Item;

        Clear();

        if (item is ICleanup cleanup)
        {
            cleanup.Release();
        }

        Released?.Invoke();
    }

    protected virtual void OnItemIncoming(Item item)
    { }

    protected virtual void OnItemStackIncoming(ItemHolder holder)
    { }
}