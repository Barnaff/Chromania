using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class BaseMenuController : MonoBehaviour {


    #region Public Properties

    public eMenuScreenType ScreenType;

    #endregion

    #region Private

    private Animator _animator;

    private System.Action _animationCompletionAction;

    [SerializeField]
    private float _fixedAnimationDuration = 1.0f;

    private bool _isAnimating = false;

    [SerializeField]
    private string _enterAnimationName;

    [SerializeField]
    private string _exitAnimationName;

    #endregion


    #region Initialize

    // Use this for initialization
    void Start () {
        _animator = this.gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("ERROR - Menu screen : " + this.gameObject.name + " missing animaotr controller");
        }
    }

    #endregion


    #region Public

    public void DisplayEnterAnimation(System.Action completionAction)
    {
        StartCoroutine(DisplayEnterAnimationCorutine(completionAction));
    }

    public IEnumerator DisplayEnterAnimationCorutine(System.Action completionAction = null)
    {
        
        float animationTimeCount = 0;
        while (_isAnimating && animationTimeCount < _fixedAnimationDuration)
        {
            animationTimeCount += Time.deltaTime;
            yield return null;
        }

        if (_animationCompletionAction != null)
        {
            _animationCompletionAction();
            _animationCompletionAction = null;
        }
    }

    public void DisplaExitnimation(System.Action completionAction)
    {
        StartCoroutine(DisplayExitAnimationCorutine(completionAction));
    }

    public IEnumerator DisplayExitAnimationCorutine(System.Action completionAction = null)
    {
        float animationTimeCount = 0;
        while (_isAnimating && animationTimeCount < _fixedAnimationDuration)
        {
            animationTimeCount += Time.deltaTime;
            yield return null;
        }

        if (_animationCompletionAction != null)
        {
            _animationCompletionAction();
            _animationCompletionAction = null;
        }
    }

    #endregion


    #region Screen Events

    public void OnEnterAnimationComplete()
    {
        _isAnimating = false;
    }

    public void OnExitAnimationComplete()
    {
        _isAnimating = false;
    }

    #endregion


  



}
