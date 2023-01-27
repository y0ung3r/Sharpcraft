using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventorySlot : ItemHolder
{
    public override void PutItem(Item item, int amount)
    {
        if (item is not InventoryItem inventoryItem)
        {
            throw new InvalidOperationException
            (
                $"{nameof(InventorySlot)} может взаимодействовать только с экземплярами {nameof(InventoryItem)}"
            );
        }

        base.PutItem(inventoryItem, amount);
    }

    protected override void OnItemIncoming(Item item)
	{
        if (item is not InventoryItem inventoryItem)
        {
            throw new InvalidOperationException
            (
                $"{nameof(InventorySlot)} может взаимодействовать только с экземплярами {nameof(InventoryItem)}"
            );
        }

        if (!inventoryItem.OwningSlot.HasItemsStack)
		{
            inventoryItem.OwningSlot
				.MoveItemTo(this);

			return;
		}

        inventoryItem.OwningSlot
			.AmountHolder
			.Amount--;

		var itemPartition = OwningContainer.Inventory
			.ItemFactory
			.Create(inventoryItem.Type);

		PutItem(itemPartition);
	}

    protected override void OnItemStackIncoming(ItemHolder holder)
        => holder.MoveItemTo(this);
}