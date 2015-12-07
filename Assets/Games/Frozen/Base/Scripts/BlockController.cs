using UnityEngine;
using System.Collections;

public delegate void AddBlockHitScoreDelegate(int score);

public class BlockController : MonoBehaviour {
	
	#region Public Properties

	public bool Indestructible;

	public Sprite[] BlockPhases;

	public GameObject ExplosionPrefab;

	public AddBlockHitScoreDelegate OnBlockAddScore;

	public int HitPoints;

	#endregion


	#region Private Properties

	private int _phaseCount = 0;

	private bool _isShaking;

	private int _hitCount = 0;

	#endregion
	

	#region Physics

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name == "Ball")
		{
			BlockHit();
		}
	}

	#endregion


	#region Public

	public void KillBlock()
	{
		if (ExplosionPrefab != null)
		{
			Vector3 position = this.gameObject.transform.position;
			position.z -= 5.0f;
			Instantiate(ExplosionPrefab, position, Quaternion.identity);
		}
		Destroy(this.gameObject);

	//	SoundUtil.PlaySound("Ice Break A", "Ice Break B", "Ice Break C_Fix", "Ice Break D_Fix");
	}

	#endregion


	#region Private 

	private void BlockHit()
	{
		if (HitPoints > 0)
		{
			_hitCount++;
			if (_hitCount > HitPoints)
			{
				KillBlock();
				return;
			}
			//SoundUtil.PlaySound("Ice Crack A", "Ice Crack B", "Ice Crack C", "Ice Crack D_Fix");

		}


		if (Indestructible)
		{
			//SoundUtil.PlaySound("Rock Bounce A", "Rock Bounce B");
			StartCoroutine(ShakeLocal(10.0f , 0.5f));
			return;
		}

		if (ExplosionPrefab != null)
		{
			Vector3 position = this.gameObject.transform.position;
			position.z -= 5.0f;
			Instantiate(ExplosionPrefab, position, Quaternion.identity);
		}

		if (BlockPhases != null && BlockPhases.Length > 0)
		{
			if (_phaseCount < BlockPhases.Length)
			{
				Sprite phaseSprite = BlockPhases[_phaseCount];
				this.gameObject.GetComponent<SpriteRenderer>().sprite = phaseSprite;
				_phaseCount++;

				if (OnBlockAddScore != null)
				{
					OnBlockAddScore(1);
				}

				return;
			}
		}

		if (OnBlockAddScore != null)
		{
			OnBlockAddScore(2);
		}

		Destroy(this.gameObject);
	}

	IEnumerator ShakeLocal(float amount, float duration)
	{
		if (!_isShaking)
		{
			_isShaking = true;
			Vector3 originalPosition = this.gameObject.transform.localPosition;
			float mod = 1.0f;
			float elapsed = 0.0f;
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime; 
				
				float percentComplete = elapsed / duration;    
				
				Vector3 amountVector = new Vector3(Random.Range(0,amount), Random.Range(0,amount), 0);
				amountVector *=  (1.0f - percentComplete) * mod;
				mod *= -1.0f;
				//amountVector = new Vector3(amountVector.x * Random.value, amountVector.y * Random.value, 0.0f);
				
				this.transform.localPosition = originalPosition + amountVector;
				
				
				yield return null;
			}
			
			this.transform.localPosition = originalPosition;
			_isShaking = false;
		}
	}

	#endregion

}
