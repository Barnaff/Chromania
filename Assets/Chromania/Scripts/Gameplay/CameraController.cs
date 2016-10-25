using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    #region Private Properties

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    private Transform _cameraTransform;
	
	// How long the object should shake for.
	private float _shake = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    [SerializeField]
    private float _shakeAmount = 0.7f;

    [SerializeField]
	private float _shakeDecreaseFactor = 1.0f;
	
	private Vector3 _originalPos;

    #endregion

    #region Initialize

    void Awake()
	{
		if (_cameraTransform == null)
		{
			_cameraTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	
	void OnEnable()
	{
		_originalPos = _cameraTransform.localPosition;
	}

    #endregion


    #region Update

    void Update()
	{
		if (_shake > 0)
		{
			_cameraTransform.localPosition = _originalPos + Random.insideUnitSphere * _shakeAmount;
			
			_shake -= Time.deltaTime * _shakeDecreaseFactor;
		}
		else
		{
			_shake = 0f;
			_cameraTransform.localPosition = _originalPos;
		}
	}

    #endregion


    #region Public

    public void Shake()
	{
		_shake = 0.3f;
	}

	public Bounds OrthographicBounds()
	{
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = this.GetComponent<Camera>().orthographicSize * 2;
		Bounds bounds = new Bounds(
			GetComponent<Camera>().transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
		return bounds;
	}

	#endregion

}
