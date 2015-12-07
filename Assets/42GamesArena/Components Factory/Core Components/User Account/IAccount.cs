using UnityEngine;
using System.Collections;

public interface IAccount  {

	void Login(System.Action completionAction);

	void UpdateDevice(System.Action completionAction);

	void Logout(System.Action completionAction);

	void ClearUserCache();
	
	bool HasLocalUser { get; }

	void FacebookLogin(string acsessToken, System.Action completionAction);

}
