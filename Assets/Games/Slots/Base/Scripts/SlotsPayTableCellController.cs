using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlotsPayTableCellController : MonoBehaviour {

	[SerializeField]
	public Image _icon;

	[SerializeField]
	public Text _infoLabel;

	public void SetIcon(SlotsIconDefenition iconDefinition)
	{
		_icon.sprite = iconDefinition.IconSprite;

		string infoText = "";

		infoText += "3 - x" + iconDefinition.BetMultiplierFor3.ToString() + "\n";
		infoText += "4 - x" + iconDefinition.BetMultiplierFor4.ToString() + "\n";
		infoText += "5 - x" + iconDefinition.BetMultiplierFor5.ToString();

		_infoLabel.text = infoText;
	}
}
