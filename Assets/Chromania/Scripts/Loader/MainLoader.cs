using UnityEngine;
using System.Collections;

public class MainLoader : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private string _version;

    [SerializeField]
    private GameObject _loaderController;

    [SerializeField]
    private ComponentFactory _componentFactoryContainer;

    #endregion


    #region Initialize

    // Use this for initialization
    void Start () {

        Application.targetFrameRate = 60;
        
        StartCoroutine(GameInitializationCorutine());
	}

    #endregion


    #region Private

    IEnumerator GameInitializationCorutine()
    {
        yield return new WaitForSeconds(1.0f);

        if (_componentFactoryContainer != null)
        {
            Instantiate(_componentFactoryContainer.gameObject);
            yield return StartCoroutine(InitializeComponents());
        } 

        MenuScreensManager.Instance().DisplayMenuScreen(eMenuScreenType.MainMenu);

        if (_loaderController != null)
        {
            Destroy(_loaderController);
        }

        yield return null;

        Destroy(this.gameObject);
    }

    IEnumerator InitializeComponents()
    {
        yield return null;
    }

    #endregion
}
