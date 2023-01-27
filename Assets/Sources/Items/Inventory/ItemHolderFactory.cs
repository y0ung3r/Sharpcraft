using UnityEngine;

[CreateAssetMenu(menuName = "Items/Views/ItemHolderFactory")]
public class ItemHolderFactory : ScriptableObject
{
    [SerializeField]
    private ItemHolder _holderTemplate;

    public ItemHolder Create(string name, ItemHolderContainer container)
    {
        var holder = Instantiate
        (
            _holderTemplate,
            container.Inventory.transform
        );

        holder.name = name;

        return holder;
    }
}
