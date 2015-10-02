using UnityEngine;
using System.Collections;

public interface IAccount  {

	void AutoLogin(System.Action <bool> completionAction);

}
