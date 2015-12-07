using UnityEngine;
using System.Collections;

public class FrozenWallController : MonoBehaviour {

	#region Public Properties

	public GameObject LeftWall;

	public GameObject RightWall;

	public GameObject TopWall;

	public GameObject BottomWall;

	#endregion


	#region Initialize

	// Use this for initialization
	void Start () {
		BuildWalls();
	}

	#endregion
	

	#region Private

	private void BuildWalls()
	{
		Vector2 screenDimenstion = CalculateScreenSizeInWorldCoords();
		LeftWall.transform.position = new Vector3(-screenDimenstion.x * 0.5f - LeftWall.GetComponent<Collider2D>().bounds.size.x * 0.5f, 0.0f , 0.0f);
		RightWall.transform.position = new Vector3(screenDimenstion.x * 0.5f + RightWall.GetComponent<Collider2D>().bounds.size.x * 0.5f, 0.0f , 0.0f);
		TopWall.transform.position = new Vector3(0.0f, screenDimenstion.y * 0.5f + TopWall.GetComponent<Collider2D>().bounds.size.y * 0.5f, 0.0f);
		BottomWall.transform.position = new Vector3(0.0f, - screenDimenstion.y * 0.5f - BottomWall.GetComponent<Collider2D>().bounds.size.y * 0.5f, 0.0f);
	}

	private Vector2 CalculateScreenSizeInWorldCoords ()  {
		Camera cam = Camera.main;
		var p1 = cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane));  
		var p2 = cam.ViewportToWorldPoint(new Vector3(1,0,cam.nearClipPlane));
		var p3 = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));
		
		float width  = (p2 - p1).magnitude;
		float height = (p3 - p2).magnitude;
		
		Vector2 dimensions = new Vector2(width,height);
		
		return dimensions;
	}

	#endregion
}
