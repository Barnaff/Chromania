using UnityEngine;
using System.Collections;


public interface IPopups  {
	
	T DisplayPopup<T>(System.Action closeAction = null) where T: class ;

	void CloseAllPopups();

	bool IsDisplayingPopup();

	PopupBaseController CurrentActivePopup();
	

}


