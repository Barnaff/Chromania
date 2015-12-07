using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlotsPayTablePopupController : MonoBehaviour {

	[SerializeField]
	private GameObject _listContainer;

	[SerializeField]
	private GameObject _cellPrefab;

	[SerializeField]
	private GameObject _linePrefab;

	private bool _isReady = false;

	void Start()
	{
		if (!_isReady)
		{
			this.gameObject.SetActive(false);
		}

		_cellPrefab.transform.SetParent(null);
		_linePrefab.transform.SetParent(null);
	}

	public void DisplayPayout(SlotsDefenition slotsDefenition)
	{
		this.gameObject.SetActive(true);

		if (_isReady)
		{
			return;
		}

		_isReady = true;



		for (int i=0; i< slotsDefenition.IconsDefenitions.Length; i++)
		{
			SlotsIconDefenition iconDefenition1 = slotsDefenition.IconsDefenitions[i];
			SlotsIconDefenition iconDefenition2 = null;
			if (i < slotsDefenition.IconsDefenitions.Length - 1)
			{
				iconDefenition2 = slotsDefenition.IconsDefenitions[i+1];
			}

			GameObject line = Instantiate(_linePrefab);

			line.transform.SetParent(_listContainer.transform);

			GameObject cell = null;

			for (int j = 0; j < line.transform.childCount; j++)
			{
				GameObject child = line.transform.GetChild(j).gameObject;
				if (child.GetComponent<SlotsPayTableCellController>() != null)
				{
					cell = child;
				}
			}

			if (cell == null)
			{
				cell = Instantiate(_cellPrefab);
			}

			if (iconDefenition1 != null)
			{
				cell.transform.SetParent(line.transform);
				cell.GetComponent<SlotsPayTableCellController>().SetIcon(iconDefenition1);	
			}

			

			if (iconDefenition2 != null)
			{
				GameObject cell2 = Instantiate(_cellPrefab);
				cell2.transform.SetParent(line.transform);
				
				cell2.GetComponent<SlotsPayTableCellController>().SetIcon(iconDefenition2);
			}
			line.transform.localScale = new Vector3(1,1,1);
			i++;
		}
	}

	public void Close()
	{
		this.gameObject.SetActive(false);
	}
}


