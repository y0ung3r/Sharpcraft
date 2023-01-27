using UnityEngine;

public class ItemHolderSelection : MonoBehaviour, ICleanup
{
	public void Select(ItemHolder holder)
	{
		AttachTo(holder);

		transform.SetAsLastSibling();
	}

	public void Release()
		=> Destroy(gameObject);

	private void AttachTo(ItemHolder holder)
		=> transform.SetParent(holder.transform, worldPositionStays: false);
}