using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class PortalConfiguration 
{
	#region Create Asset

#if UNITY_EDITOR
	[MenuItem("42 Games Arena/Create/Portal Configuration")]

	public static void CreateAsset ()
	{
		//ScriptableObjectUtility.CreateAsset<PortalConfiguration> ();
	}
#endif

	#endregion


	#region Public Properties

	[SerializeField]
	public string PortalIdentifier;

	[SerializeField]
	public float Version;

	[SerializeField]
	public string ServerURL;

	[SerializeField]
	public LobbyDataModel PortalData;

	#endregion
}