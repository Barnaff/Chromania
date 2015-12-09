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

        GameObject mainMenuControllerPrefab = Resources.Load("MainMenuController") as GameObject;
        if (mainMenuControllerPrefab != null)
        {
            GameObject mainMenuController = Instantiate(mainMenuControllerPrefab) as GameObject;
            mainMenuController.transform.position = Vector3.zero;
        }

        if (_loaderController != null)
        {
            Destroy(_loaderController);
        }
    }

    #endregion
}
