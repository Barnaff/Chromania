using UnityEngine;
using System.Collections;

public class BubbleDepthBombController : MonoBehaviour {

	#region Public Properties

	public GameObject ExplosionPrefab;

	#endregion



	#region Public

	public void ExplodeBomb()
	{
		if (ExplosionPrefab != null)
		{
			Instantiate(ExplosionPrefab, this.transform.position, Quaternion.identity);
		}

		this.gameObject.SetActive(false);

		//SoundUtil.PlaySound("Canon shoot D_Fix");
	}

	#endregion
}
