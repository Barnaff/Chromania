using UnityEngine;
using System;

public class ComponentsFactory : MonoBehaviour {

	#region Public Properties

	/// <summary>
	/// The container.
	/// </summary>
	public static GameObject Container;

	#endregion


	#region Initialize
	
	void Awake()
	{
		ComponentsFactory.Container = this.gameObject;
		DontDestroyOnLoad(this.gameObject);
	}

	#endregion


	#region Public

	/// <summary>
	/// Gets A component.
	/// </summary>
	/// <returns>The A component.</returns>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T GetAComponent<T>() where T: class 
	{
		T component = ComponentsFactory.Container.GetComponent(typeof(T)) as T;
		if (component != null)
		{
			return component;
		}
		
		Debug.LogError("ERROR - Could not load component for type: " + typeof(T).ToString());
		
		return null;
	}

	#endregion
}
