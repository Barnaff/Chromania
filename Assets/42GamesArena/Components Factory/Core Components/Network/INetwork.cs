using System.Collections;

public delegate void NetworkResultDelegate(Hashtable response);

public interface INetwork  {

	event NetworkResultDelegate OnNetworkResult;

	void PostServerCommand(string command, Hashtable data, System.Action <Hashtable> sucsessBlock,  System.Action <ServerError> failBlock);
	void PostServerCommand(string command, Hashtable data, RequestMethodType methodType, System.Action <Hashtable> sucsessBlock,  System.Action <ServerError> failBlock);

}


