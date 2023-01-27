using System;
using System.Linq;
using UnityEngine;

public class ItemHolderContainer : MonoBehaviour
{
    [SerializeField]
    private ItemHolder[] _holders;

    [SerializeField]
    private Inventory _inventory;

    public Inventory Inventory
        => _inventory;
    
    public ItemHolder GetHolder(int index)
        => _holders[index];

    public int IndexOf(ItemHolder holder)
        => Array.IndexOf(_holders, holder);

    public bool HasHolder(int index)
        => index >= 0 && index < _holders.Length;

    public ItemHolder GetFirstFreeHolder(int startIndex)
        => _holders.Skip(startIndex)
                .FirstOrDefault(holder => !holder.HasItem);

    public ItemHolder GetFirstHolder(Func<ItemHolder, bool> predicate)
        => _holders.FirstOrDefault(predicate);

    public void Awake()
    {
        var views = GetComponentsInChildren<ItemHolder>();
        _holders = new ItemHolder[views.Length];

        for (var holderIndex = 0; holderIndex < views.Length; holderIndex++)
        {
            var holder = _holders[holderIndex] = views[holderIndex];
            holder.ItemPlaced += OnItemPlaced;
            holder.Cleaned += OnItemDisplaced;
        }

        Inventory.DragBeginned += OnHolderDragBeginned;
    }

    protected virtual void OnHolderDragBeginned(Item item)
    {
        // TODO: ?

        if (item is not InventoryItem inventoryItem)
        {
            throw new InvalidOperationException
            (
                $"{nameof(ItemHolderContainer)} может взаимодействовать только с экземплярами {nameof(InventoryItem)}"
            );
        }

        if (inventoryItem.OwningSlot.OwningContainer != this || !HasHolder(inventoryItem.OwningSlot.Index))
        {
            return;
        }

        OnItemDisplaced(inventoryItem.OwningSlot.Index, item);
    }

    protected virtual void OnItemPlaced(int holderIndex, Item item, int amount)
    { }

    protected virtual void OnItemDisplaced(int holderIndex, Item item)
    { }
}