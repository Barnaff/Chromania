using UnityEngine;
using System.Collections;

public class MainLoader : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private string _version;

    [SerializeField]
    private GameObject _loaderController;

    #endregion


    #region Initialize

    // Use this for initialization
    void Start () {


        StartCoroutine(GameInitializationCorutine());
	}

    #endregion



    #region Private

    IEnumerator GameInitializationCorutine()
    {
        yield return new WaitForSeconds(1.0f);

        MenuScreensManager.Instance().DisplayMenuScreen(eMenuScreenType.MainMenu);

        if (_loaderController != null)
        {
            Destroy(_loaderController);
        }

        yield return null;

        Destroy(this.gameObject);
    }

    #endregion
}
