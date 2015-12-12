using UnityEngine;
using System.Collections;

public abstract class FactoryComponent : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartComponent();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract IEnumerator InitComponent();

    public abstract void StartComponent();
}
