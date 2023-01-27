using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryItem : Item, IAttachable, ICleanup
{
	private Image _itemImage;

	public InventorySlot OwningSlot
		=> GetComponentInParent<InventorySlot>();

	public void Awake()
		=> _itemImage = GetComponent<Image>();

    public void AttachTo(ItemHolder holder)
		=> transform.SetParent(holder.transform, worldPositionStays: transform != transform.root);

    public void Detach()
		=> transform.SetParent(transform.root);

    public void Release()
		=> Destroy(gameObject);

	protected override void OnRender(ItemAsset asset)
		=> _itemImage.sprite = asset.Icon;
}