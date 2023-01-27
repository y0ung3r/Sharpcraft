using UnityEngine;

public class EquipmentSlot : InventorySlot
{
    [SerializeField]
    private EquipmentPart _equipmentPart;

    public override void PutItem(Item item, int amount)
    {
        if (item.EquipmentPart == _equipmentPart)
        {
            base.PutItem(item, amount);
        }
    }

    protected override void OnItemIncoming(Item item)
    {
        if (item.EquipmentPart == _equipmentPart)
        {
            base.OnItemIncoming(item);
        }
    }

    protected override void OnItemStackIncoming(ItemHolder holder)
        => OnItemIncoming(holder.Item);
}