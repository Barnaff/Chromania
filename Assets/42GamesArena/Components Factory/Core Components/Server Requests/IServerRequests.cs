using System.Collections;

public interface IServerRequests  {


	void SendServerRequest(string command, Hashtable data, System.Action <Hashtable> completionAction, System.Action <ServerError> failAction, bool authonticate = true);

	void ClearLocalUser();
}
