using UnityEngine;
using System.Collections;

public delegate void BubbleDepthShootBallDelegate(GameObject ball);

public class BubbleDepthCannonController : MonoBehaviour {

	#region Public Properties

	public float ShootForce = 5.0f;

	public BubbleDepthShootBallDelegate OnShootBall;

	public BubbleDepthSwapButtonController SwapButton;

	public GameObject AimObject;

	public bool Paused;

	#endregion


	#region Private Properties

	private GameObject _armedBall;

	private GameObject _secondBall;

	#endregion


	#region Initialize

	void Start()
	{
		SwapButton.OnSwapButtonPressed += SwapBalls;
		AimObject.SetActive(false);
	}

	#endregion


	#region Update
	
	// Update is called once per frame
	void Update () {
	
		if (Paused)
		{
			return;
		}
#if UNITY_EDITOR
		Vector3 mouse_pos = Input.mousePosition;

		mouse_pos.z = 10.0f;
		Vector3 object_pos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);

		if (mouse_pos.y > object_pos.y + 50.0f)
		{
			mouse_pos.x = mouse_pos.x - object_pos.x;
			mouse_pos.y = mouse_pos.y - object_pos.y;
			float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
			
			if (Input.GetMouseButtonDown(0))
			{
				Shoot();
			}
		}



	

#elif UNITY_IPHONE || UNITY_ANDROID
		
		if (Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];
			Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, 10.0f);
			Vector3 object_pos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);

			switch (touch.phase)
			{
			case TouchPhase.Began:
			case TouchPhase.Moved:
			case TouchPhase.Stationary:
			{
			
				if (touchPosition.y > object_pos.y + 50.0f)
				{
					touchPosition.x = touchPosition.x - object_pos.x;
					touchPosition.y = touchPosition.y - object_pos.y;
					float angle = Mathf.Atan2(touchPosition.y, touchPosition.x) * Mathf.Rad2Deg;
					transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
				}
				break;
			}
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
			{
				if (touchPosition.y > object_pos.y + 50.0f)
				{
					Shoot();
				}
				break;
			}
			}
		}
#endif


		if (_secondBall != null)
		{
			_secondBall.SetActive(true);
		}

		if (_armedBall != null)
		{
			_armedBall.SetActive(true);
		}
	}

	void FixedUpdate() {

//		RaycastHit2D hit = Physics2D.Raycast(AimObject.transform.position, AimObject.transform.right);
//
//		if (hit) {            
//			Debug.Log(hit.collider);
//
//			Debug.DrawRay(hit.point, hit.normal);
//		}
//		
//		Debug.DrawRay(AimObject.transform.position, AimObject.transform.right, Color.red);

	}

	#endregion


	#region Public

	public void ArmCannon(GameObject ball , GameObject secondBall = null)
	{
		AimObject.SetActive(true);
		bool animateSwap = false;
		if (secondBall != null)
		{
			Destroy(_secondBall);
			_secondBall = null;
			_secondBall = secondBall;
			_armedBall = ball;
		}
		else
		{
			animateSwap = true;
			_armedBall = _secondBall;
			_secondBall = ball;
		}


		PlaceBalls(animateSwap);

	}

	public void ClearCannon()
	{
		Destroy(_armedBall);
		Destroy(_secondBall);
		_armedBall = null;
		_secondBall = null;
	}

	#endregion


	#region Private

	private void Shoot()
	{
		if (_armedBall)
		{
			AimObject.SetActive(false);
			Vector2 force = new Vector2(Mathf.Cos(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * ShootForce, 
			                            Mathf.Sin(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * ShootForce);

			_armedBall.GetComponent<Collider2D>().isTrigger = false;
			_armedBall.GetComponent<Rigidbody2D>().isKinematic = false;
			_armedBall.GetComponent<Rigidbody2D>().velocity = force;


			if (OnShootBall != null)
			{
				OnShootBall(_armedBall);
			}
			_armedBall = null;

			//SoundUtil.PlaySound("Canon Shoot B");
		}
	}

	private void SwapBalls()
	{
		GameObject tmpBall = _secondBall;
		_secondBall = _armedBall;
		_armedBall = tmpBall;

		PlaceBalls(true);

		//SoundUtil.PlaySound("Bubble Change");
	}

	private void PlaceBalls(bool animated)
	{
		if (_armedBall != null)
		{
			_armedBall.SetActive(true);
			_armedBall.transform.SetParent(this.gameObject.transform);

			Vector3 position = this.gameObject.transform.position;
			_armedBall.GetComponent<Collider2D>().isTrigger = true;
			if (animated)
			{
				iTween.Stop(_armedBall);
				iTween.MoveTo(_armedBall, iTween.Hash("time", 0.3f, "x", position.x, "y", position.y, "z", position.z ));
			}
			else
			{
				_armedBall.transform.position = position;
			}
		}

		if (_secondBall != null)
		{
			_secondBall.SetActive(true);
			_secondBall.transform.SetParent(null);

			_secondBall.GetComponent<Collider2D>().isTrigger = true;
			_secondBall.SetActive(true);
			Vector3 position = SwapButton.transform.position;
			position.x -= 0.7f;
			if (animated)
			{
				if (Vector3.Distance(_secondBall.transform.position, position) > 2.0f)
				{
					_secondBall.transform.position = position;
					iTween.Stop(_secondBall);
					iTween.PunchScale(_secondBall, iTween.Hash("time", 0.3f, "x", 1f, "y", 1f));
				}
				else
				{
					iTween.Stop(_secondBall);
					iTween.MoveTo(_secondBall, iTween.Hash("time", 0.3f, "x", position.x, "y", position.y, "z", position.z ));
				}
			}
			else
			{
				_secondBall.transform.position = position;
			}
		}
	}

	#endregion
}
