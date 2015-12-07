using UnityEngine;
using System.Collections;

public delegate void BalltHitTopDelegate(FrozenBallController ballController);
public delegate void FrozenBallHitBottomDelegate(FrozenBallController ballController);

public class FrozenBallController : MonoBehaviour {
	
	#region Public Properties

	public float BallSpeed = 100.0f;

	public BalltHitTopDelegate OnHitTop;

	public FrozenBallHitBottomDelegate OnHitBottom;

	#endregion


	#region Initialize
	
	void Start() 
	{
		this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}

	#endregion

	
	#region Public

	public void ShootBullet()
	{
		this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * BallSpeed;
		this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(this.gameObject.GetComponent<Rigidbody2D>().velocity.x + Random.Range(-5,5), this.gameObject.GetComponent<Rigidbody2D>().velocity.y);
	}

	#endregion


	#region Physics

	void OnCollisionEnter2D(Collision2D collision) {
		// Hit the Racket?
		if (collision.gameObject.name == "Player Controller") {
			// Calculate hit Factor
			float x = HitFactor(transform.position, collision.transform.position,((BoxCollider2D)collision.collider).size.x);
			
			// Calculate direction, set length to 1
			Vector2 direction = new Vector2(x, 1).normalized;
			
			// Set Velocity with dir * speed
			this.gameObject.GetComponent<Rigidbody2D>().velocity = direction * BallSpeed;
		}

		if (collision.gameObject.name == "Top Wall")
		{
			if (OnHitTop != null)
			{
				OnHitTop(this);
			}
		}
		if (collision.gameObject.name == "Bottom Wall")
		{
			if (OnHitBottom != null)
			{
				OnHitBottom(this);
			}
		}
	}

	private float HitFactor(Vector2 ballPos, Vector2 playerPos,float polayerWidth) {

		return (ballPos.x - playerPos.x) / polayerWidth;
	}

	#endregion

}
