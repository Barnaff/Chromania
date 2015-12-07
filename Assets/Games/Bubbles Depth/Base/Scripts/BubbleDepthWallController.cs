using UnityEngine;
using System.Collections;

public class BubbleDepthWallController : MonoBehaviour {

	#region Public Properties

	/// <summary>
	/// The left wall.
	/// </summary>
	public GameObject LeftWall;

	/// <summary>
	/// The right wall.
	/// </summary>
	public GameObject RightWall;

	/// <summary>
	/// The top wall.
	/// </summary>
	public GameObject TopWall;

	/// <summary>
	/// The bottom wall.
	/// </summary>
	public GameObject BottomWall;
	
	#endregion

	// Use this for initialization
	void Start () {
	
		Vector3 leftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f));
		leftCorner.z = 0;
		float screenWidth = Mathf.Abs(leftCorner.x) * 2.0f;
		float screeHeight = Mathf.Abs(leftCorner.y) * 2.0f;

		if (LeftWall != null)
		{
			LeftWall.transform.position = new Vector3(-screenWidth * 0.5f - LeftWall.GetComponent<Collider2D>().bounds.size.x * 0.5f, 0, 0);
		}

		if (RightWall != null)
		{
			RightWall.transform.position = new Vector3(screenWidth * 0.5f + RightWall.GetComponent<Collider2D>().bounds.size.x * 0.5f, 0, 0);
		}

		if (TopWall != null)
		{
			TopWall.transform.position = new Vector3(0, screeHeight * 0.5f + TopWall.GetComponent<Collider2D>().bounds.size.y * 0.5f, 0);
		}

		if (BottomWall != null)
		{
			BottomWall.transform.position = new Vector3(0, -screeHeight * 0.5f - BottomWall.GetComponent<Collider2D>().bounds.size.y * 0.5f, 0);
		}
	}


}
