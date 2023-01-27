using TMPro;
using UnityEngine;

public class AmountHolder : MonoBehaviour
{
	public const int MaxAmount = 64;
	
	[SerializeField]
	private TMP_Text _printer;
	
	private int _amount = 0;

    public bool HasStack
        => _amount > 1;

    public bool IsVisible
	{
		get => _printer.gameObject.activeSelf;
		
		set
		{
			var canVisible = value && HasStack;

			if (canVisible)
			{
				_printer.transform
					.SetAsLastSibling();
			}
			
			_printer.gameObject
				.SetActive(canVisible);
		}
	}
	
	public int Amount
	{
		get => _amount;

		set
		{
			if (value > MaxAmount)
			{
				throw new HolderIsFullException();
			}

			_amount = value;
			_printer.text = _amount.ToString();

			IsVisible = true;
		}
	}
}