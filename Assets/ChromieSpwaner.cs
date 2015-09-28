using UnityEngine;
using System.Collections;

public class ChromieSpwaner : MonoBehaviour {

	public GameObject chromiePrefab;
	public GameObject colorZonePrefab;

	// Use this for initialization
	void Start () {
		createColorZones();
		//spwanChromie();

		InvokeRepeating("spwanChromie", 1.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void spwanChromie()
	{
		Vector3 spwanPosition = new Vector3(0,-10.0f,0);
		GameObject newChromie = (GameObject)Instantiate(chromiePrefab ,spwanPosition, Quaternion.identity);
		Vector3 spwanForce = Vector3.up * Random.Range(800, 950);
		spwanForce.x = Random.Range(-40,40);
		newChromie.GetComponent<Rigidbody>().AddForce(spwanForce);

		Destroy(newChromie, 5.0f);
	}

	private void createColorZones()
	{
		createColorZone(1, new Vector3(0,0,10));
		createColorZone(1, new Vector3(1,0,10));
		createColorZone(1, new Vector3(1,1,10));
		createColorZone(1, new Vector3(0,1,10));
	}

	private GameObject createColorZone(int color, Vector3 position)
	{
		position.x *= Screen.width;
		position.y *= Screen.height;
		position = GetComponent<Camera>().ScreenToWorldPoint(position);
		GameObject colorZone = (GameObject)Instantiate(colorZonePrefab, position, Quaternion.identity);
		return colorZone;
	}
}
