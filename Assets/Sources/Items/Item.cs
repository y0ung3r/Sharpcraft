using UnityEngine;

public abstract class Item : MonoBehaviour
{
	public ItemType Type { get; private set; }

    public EquipmentPart EquipmentPart { get; private set; }

	public bool Stackable { get; private set; }

    public void Render(ItemAsset asset)
	{
		Type = asset.Type;
		EquipmentPart = asset.EquipmentPart;
		Stackable = asset.Stackable;

        OnRender(asset);
	}

    protected abstract void OnRender(ItemAsset asset);
}