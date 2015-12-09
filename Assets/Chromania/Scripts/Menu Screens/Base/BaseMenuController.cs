using UnityEngine;
using System.Collections;

public class BaseMenuController : MonoBehaviour {

    private Animator _animator;

    // Use this for initialization
    void Start () {
        _animator = this.gameObject.GetComponent<Animator>();
        if (_animator != null)
        {
            Debug.LogError("ERROR - Menu screen missing animaotr controller");
        }
    }
	
	// Update is called once per frame
	void Update () {

	}


	public virtual void OnEnterAnimationComplete() {}

	public virtual void OnExitAnimationComplete() {}

}
