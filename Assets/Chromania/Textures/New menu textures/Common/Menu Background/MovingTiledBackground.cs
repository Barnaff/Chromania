using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovingTiledBackground : MonoBehaviour {

    [SerializeField]
    private Vector2 _speed = new Vector2(1.0f, 1.0f);

    private Material _backgroundMaterial;

	// Use this for initialization
	void Start () {

        Image image = this.gameObject.GetComponent<Image>();
        _backgroundMaterial = image.material;
	}
	
	// Update is called once per frame
	void LateUpdate () {
	
        if (_backgroundMaterial != null)
        {
            Vector2 offset = _backgroundMaterial.mainTextureOffset;
            _backgroundMaterial.mainTextureOffset = new Vector2(offset.x + (Time.deltaTime * _speed.x), offset.y + (Time.deltaTime * _speed.y));
        }
	}
}
