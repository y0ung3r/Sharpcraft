public class Equipment : ItemHolderContainer
{
    public ItemHolder Head
        => GetEquipmentPart(EquipmentPart.Head);

    public ItemHolder Body
        => GetEquipmentPart(EquipmentPart.Body);

    public ItemHolder Legs
        => GetEquipmentPart(EquipmentPart.Legs);

    public ItemHolder Feet
        => GetEquipmentPart(EquipmentPart.Feet);

    public ItemHolder GetEquipmentPart(EquipmentPart part)
    {
        if (part == EquipmentPart.None)
        {
            return null;
        }

        return GetHolder((int)part);
    }
}
