public class ItemDropEntry : InventorySlot
{
    protected override void OnItemIncoming(Item item)
    {
        if (item is InventoryItem inventoryItem)
        {
            inventoryItem.OwningSlot.DropItem();
        }
    }

    protected override void OnItemStackIncoming(ItemHolder holder)
        => holder.DropItem(holder.AmountHolder.Amount);
}