using UnityEngine;
using System.Collections;

public class ChromieInitiatorController : MonoBehaviour {

    private System.Action _completionAction;
    private float _delay;
    private int _index;
    private Vector3 _originalPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisplayChromieIntro(int index, float delay,  System.Action completionAction)
    {
        _completionAction = completionAction;
        _delay = delay;
        _index = index;
        _originalPosition = this.gameObject.transform.position;
        StartCoroutine(ChromieRandomMovememnt());
    }

    IEnumerator ChromieRandomMovememnt()
    {
        RandomMovement();

        yield return new WaitForSeconds(_delay);

        iTween.Stop(this.gameObject);

        yield return new WaitForEndOfFrame();

        ColorZonesManager colorZonesManager = GameObject.FindObjectOfType<ColorZonesManager>() as ColorZonesManager;
        if (colorZonesManager != null)
        {
            Vector3 colorZonePosition = colorZonesManager.PositionForColorZone(_index);
            iTween.MoveTo(this.gameObject, iTween.Hash("time", 0.5f, "x", colorZonePosition.x, "y", colorZonePosition.y, "oncomplete", "FinishedMovingToColorZone", "oncompletetarget", this.gameObject, "easetype", iTween.EaseType.easeInSine));
        }

        yield return null;
    }

    private void RandomMovement()
    {
        Vector3 position = _originalPosition + Random.insideUnitSphere * 0.2f;
        float randomTime = UnityEngine.Random.Range(0.1f, 0.5f);
        iTween.MoveTo(this.gameObject, iTween.Hash("time", randomTime, "x", position.x, "y", position.y, "easetype", iTween.EaseType.linear, "oncomplete", "RandomMovement", "oncompletetarget", this.gameObject));
    }

    private void FinishedMovingToColorZone()
    {
        if (_completionAction != null)
        {
            _completionAction();
        }
        this.gameObject.SetActive(false);
    }
}
