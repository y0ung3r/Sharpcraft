using UnityEngine;

public class GuiEntry : MonoBehaviour
{
	private Inventory _inventory;

	[Header("Hotkeys")] 
	[SerializeField] private KeyCode _openInventoryKey;
	[SerializeField] private KeyCode[] _slotKeys;

	public void Start()
	{
		_inventory = GetComponentInChildren<Inventory>(includeInactive: true);
		
		_inventory.Toolbar
			.Select(index: 0);
	}

	public void Update()
	{
		HandleInventoryOpenKeyInput();
		HandleSlotKeysInput();
	}

	private void HandleInventoryOpenKeyInput()
	{
		if (Input.GetKeyDown(_openInventoryKey))
		{
			_inventory.IsOpen = !_inventory.IsOpen;
		}
	}

	private void HandleSlotKeysInput()
	{
		if (_inventory.IsOpen)
		{
			return;
		}

		for (var slotKeyIndex = 0; slotKeyIndex < _slotKeys.Length; slotKeyIndex++)
		{
			if (Input.GetKeyDown(_slotKeys[slotKeyIndex]))
			{
				_inventory.Toolbar.Select(slotKeyIndex);
			}
		}
	}
}