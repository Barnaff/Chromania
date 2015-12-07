using UnityEngine;
using System.Collections;

public delegate void BubbleDepthBallHitDelegate(BubbleDepthBallController ball, BubbleDepthBallController hit);

public delegate void BubbleDepthBallHitBottomDelegate(BubbleDepthBallController ball);

public delegate void BubbleDepthBallHitSinkingObjectDelegate(BubbleDepthBallController ball);

public delegate void BubbleDepthBallHitBombDelegate(BubbleDepthBallController ball, GameObject bomb);

public class BubbleDepthBallController : MonoBehaviour {

	#region Public Properties

	public Vector2 PositionInBoard;

	public bool IsShifted;

	public BubbleDepthBallHitDelegate OnBallHit;

	public BubbleDepthBallHitBottomDelegate OnHitBottom;

	public BubbleDepthBallHitSinkingObjectDelegate OnHitSinkingObject;

	public BubbleDepthBallHitBombDelegate OnBallHitBomb;

	public bool Checked;

	public GameObject ExplosionPrefab;


	#endregion


	public void KillBall()
	{
		if (ExplosionPrefab != null)
		{
			GameObject explosion =  Instantiate(ExplosionPrefab) as GameObject;
			explosion.transform.position = this.gameObject.transform.position;

			//string[] soundsArray = new string[2]{"Bubble Pop A", "Bubble Pop B"};
			//SoundUtil.PlaySound(soundsArray[Random.Range(0,soundsArray.Length)]);
		}
	}

	#region Physics

	void OnCollisionEnter2D(Collision2D collision)
	{
		BubbleDepthBallController otherBall = collision.gameObject.GetComponent<BubbleDepthBallController>() as BubbleDepthBallController;
		if (otherBall != null)
		{
			this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			this.GetComponent<Rigidbody2D>().isKinematic = true;

			if (OnBallHit != null)
			{
				OnBallHit(this, otherBall);
			}
		}

		if (collision.gameObject.name == "Skinking Object")
		{
			Debug.Log("hit Skinking Object");
			if (OnHitSinkingObject != null)
			{
				OnHitSinkingObject(this);
			}
		}

		if (collision.gameObject.name == "Bomb")
		{
			if (OnBallHitBomb != null)
			{
				OnBallHitBomb(this, collision.gameObject);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.name == "Bottom Wall")
		{
			if (OnHitBottom != null)
			{
				OnHitBottom(this);
			}
		}
	}


	#endregion

}
