using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LivesGUIController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private Text _livesTextField;

    [SerializeField]
    private int _currentLives;

    [SerializeField]
    private int _maxLives;

    #endregion

    // Use this for initialization
    void Start () {

        GameplayEventsDispatcher.Instance.OnLivesUpdate += OnLivesUpdateHandler;
	}

    #region Events

    private void OnLivesUpdateHandler(int maxLives, int currentLives)
    {
        _currentLives = currentLives;
        _maxLives = maxLives;

        if (_livesTextField != null)
        {
            _livesTextField.text = "Lives: " + _currentLives + "/" + _maxLives;
        }
    }

    #endregion
}
