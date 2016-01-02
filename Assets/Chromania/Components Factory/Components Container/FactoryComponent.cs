using UnityEngine;
using System.Collections;

public abstract class FactoryComponent : MonoBehaviour {

	// Use this for initialization
	void Awake ()
    {
        InitComponentAtAwake();
    }

    void Start()
    {
        InitComponentAtStart();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract void InitComponentAtStart();
    public abstract void InitComponentAtAwake();

}
