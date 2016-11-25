using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayIntroController : MonoBehaviour {

    #region Public Properties

    public delegate void FinishedChromieEnterAnimationDelegate(int chromieIndex);

    public event FinishedChromieEnterAnimationDelegate OnFinishedChromieEnterAnimation;

    #endregion

    #region Private Properties

    [SerializeField]
    private GameObject[] _chromieContainers;

    [SerializeField]
    private ChromieController _chromieController;

    #endregion

    // Use this for initialization
    void Start () {

        DisplayIntro(GameSetupManager.Instance.SelectedChromiez);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Public

    public void DisplayIntro(List<ChromieDefenition> selectedChromiez)
    {
        for (int i=0; i< selectedChromiez.Count; i++)
        {
            GameObject chromieCharacter = Instantiate(selectedChromiez[i].CharacterPrefab) as GameObject;
            chromieCharacter.transform.SetParent(_chromieContainers[i].transform);
            chromieCharacter.transform.localPosition = Vector3.zero;
            chromieCharacter.transform.localScale = Vector3.one;
            
        }
    }

    public void FinishedMovingChromie(int index)
    {
        Debug.Log("finished moving: " + index);

        if (OnFinishedChromieEnterAnimation != null)
        {
            OnFinishedChromieEnterAnimation(index);
        }
    }

    #endregion
}
