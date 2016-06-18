using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayIntroController : MonoBehaviour {

    #region private properties

    private eChromieType[] _selectedColors;

    private System.Action _completionAction;

    private List<ChromieController> _introDisplayChromiezList;

    #endregion


    #region Public

    public static GameplayIntroController DisplayGameplayIntro(eChromieType[] selectedColors, eGameMode gameMode, System.Action completionAction)
    {
        GameObject gameIntroContainer = new GameObject();
        gameIntroContainer.name = "Gameplay Intro";
        GameplayIntroController gameplayIntroController = gameIntroContainer.AddComponent<GameplayIntroController>();
        gameplayIntroController._selectedColors = selectedColors;
        gameplayIntroController._completionAction = completionAction;
        gameplayIntroController.StartCoroutine(gameplayIntroController.DisplayGameplayIntroCorutine());
        return gameplayIntroController;
    }


    #endregion


    #region private

    private IEnumerator DisplayGameplayIntroCorutine()
    {
        yield return new WaitForSeconds(1.0f);

        SpwanerController spwanerController = GameObject.FindObjectOfType<SpwanerController>();

        ColorZonesManager colorZonesManager = GameObject.FindObjectOfType<ColorZonesManager>();

       

        for (int i = 0; i < _selectedColors.Length; i++)
        {
            eChromieType chromieType = _selectedColors[i];
            ChromieController chromieController = spwanerController.CreateChromie(chromieType);
            chromieController.gameObject.transform.position = new Vector3(-3.0f + (i * 2.0f), 0, 0);
            chromieController.GetComponent<Rigidbody2D>().isKinematic = true;
            chromieController.gameObject.GetComponent<Collider2D>().enabled = false;

            ColorZoneController colorZoneController = colorZonesManager.ColorZoneForColor(chromieType);

            StartCoroutine(IntroForChromie(chromieController, colorZoneController, i));

         
        }
       

        yield return new WaitForSeconds(5.0f);

        if (_completionAction != null)
        {
            _completionAction();
        }

        Destroy(this.gameObject);
    }


    IEnumerator IntroForChromie(ChromieController chromieController, ColorZoneController colorZoneController, int index)
    {
        iTween.MoveFrom(chromieController.gameObject, iTween.Hash("time", 0.5f + Random.Range(0.0f, 0.2f), "position", Vector3.zero, "easetype", iTween.EaseType.easeOutElastic));
        iTween.ScaleFrom(chromieController.gameObject, iTween.Hash("time", 0.8f, "scale", Vector3.zero, "easetype",  iTween.EaseType.easeOutElastic));
        colorZoneController.SetForIntro();


        yield return new WaitForSeconds(1.5f + (index * 0.5f));


        iTween.MoveTo(chromieController.gameObject, iTween.Hash("time", 0.5f, "position", colorZoneController.gameObject.transform.position, "easetype", iTween.EaseType.easeInCirc));

        yield return new WaitForSeconds(0.5f);

        chromieController.gameObject.SetActive(false);

        colorZoneController.DisplayIntroAnimation(() =>
        {

        });
    }

    #endregion
}
