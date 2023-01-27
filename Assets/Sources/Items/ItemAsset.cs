using UnityEngine;

[CreateAssetMenu(menuName = "Items/Asset")]
public class ItemAsset : ScriptableObject
{
	[SerializeField]
	private ItemType _type;

	[SerializeField]
	private EquipmentPart _equipmentPart;
	
	[SerializeField] 
	private Sprite _icon;

	[SerializeField]
	private Material _material;

	[SerializeField]
	private bool _stackable;

	public ItemType Type 
		=> _type;

	public EquipmentPart EquipmentPart 
		=> _equipmentPart;

	public bool Stackable 
		=> _stackable;

	public Sprite Icon 
		=> _icon;

	public Material Material 
		=> _material;
}