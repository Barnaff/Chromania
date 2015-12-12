using UnityEngine;

public class ComponentFactory : MonoBehaviour {

	public static GameObject Container;

	public static bool DisplayExeptions = false;
	
	void Awake()
	{
		ComponentFactory.Container = this.gameObject;
		DontDestroyOnLoad(this.gameObject);
	}

	public static T GetAComponent<T>() where T: class 
	{
		try
		{
			T component = ComponentFactory.Container.GetComponent(typeof(T)) as T;
			if (component != null)
			{
				return component;
			}
		}
		catch (System.Exception e)
		{
			if (DisplayExeptions)
			{
				Debug.LogError("ERROR - Could not load component for type: " + typeof(T).ToString() + " stack: " +  e);
			}
		}

		return null;
	}
}

