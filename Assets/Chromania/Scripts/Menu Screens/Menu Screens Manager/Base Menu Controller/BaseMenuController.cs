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

    [SerializeField]
    private string _enterAnimationName;

    [SerializeField]
    private string _exitAnimationName;

    #endregion


    #region Initialize

    // Use this for initialization
    void Awake () {
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
        _animator.Play(_enterAnimationName);
        float animationDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
        float animationTimeCount = 0;
        while (animationTimeCount < animationDuration)
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
        _animator.Play(_exitAnimationName);

        float animationDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
        float animationTimeCount = 0;
        while (animationTimeCount < animationDuration)
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


    #region Private


    #endregion



}
