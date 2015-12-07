using UnityEngine;
using System.Collections;

public delegate void OnBackButtonMethod();

public interface IBackButton  {
	
	void RegisterToBackButton(OnBackButtonMethod callBack);

	void RemoveResponderFromBackButton(OnBackButtonMethod callBack);
}
