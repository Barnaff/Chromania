using UnityEngine;
using System.Collections;

public class BaseMenuController : MonoBehaviour {


    #region Public Properties

    public eMenuScreenType ScreenType;

    #endregion

    #region Private

    private Animator _animator;

    #endregion


    #region Initialize

    // Use this for initialization
    void Start () {
        _animator = this.gameObject.GetComponent<Animator>();
        if (_animator != null)
        {
            Debug.LogError("ERROR - Menu screen missing animaotr controller");
        }
    }

    #endregion


    #region Subclassing

    public virtual void OnEnterAnimationComplete() {}

	public virtual void OnExitAnimationComplete() {}

    #endregion
}
