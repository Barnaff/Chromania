using UnityEngine;
using System.Collections;

public class GameplayTutorialController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private GameObject _tutorialDragIndicator;

    #endregion

    // Use this for initialization
    void Start () {

        GameplayController gameplayController = GameObject.FindObjectOfType<GameplayController>();

        Debug.Log(">>>>> " + gameplayController);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
