using UnityEngine;
using System.Collections;

public class SpwanerController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private eChromieType[] _selectedChromies;

    [SerializeField]
    private Vector3 _spwanBasePosition;

    private const float FORCE_VECTOR_MODIFIER = 450;
    private const float SPWAN_POSITION_MULTIPLIER = 0.032f;

    #endregion


    // Use this for initialization
    void Start () {

        GameplayEventsDispatcher.Instance().OnChromieDropped += ChromieDroppedHandler;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Public 

    public void Init(eChromieType[] selectedCollors, int level)
    {
        _spwanBasePosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, 0, 10));

        _selectedChromies = selectedCollors;

        InvokeRepeating("MockSpwan", 1.0f, 1.0f);
    }

    #endregion


    private void MockSpwan()
    {
        eChromieType randomChromie = GetRandomChromieForSpwan();
        Vector3 spwanPosition = Vector3.zero;
        Vector2 forceVector = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        SpwanChromie(randomChromie, spwanPosition, forceVector);
    }

    private void SpwanChromie(eChromieType chromieType, Vector3 position, Vector2 direction)
    {
        GameObject chromiePrefab = GetChromiePrefab(chromieType);
        GameObject newChromie = Lean.LeanPool.Spawn(chromiePrefab);
        ChromieController chromieController = newChromie.GetComponent<ChromieController>();
        chromieController.Init();

        Vector3 spwanPosition = _spwanBasePosition;
        spwanPosition.x += position.x * SPWAN_POSITION_MULTIPLIER;
        newChromie.transform.position = spwanPosition;
        direction.y += FORCE_VECTOR_MODIFIER;
        newChromie.GetComponent<Rigidbody2D>().AddForce(direction);
        newChromie.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-90, 90);

        GameplayEventsDispatcher.SendChromieSpwaned(chromieController);
    }

    private GameObject GetChromiePrefab(eChromieType chromieType)
    {
        IChromiezAssetsCache chromiezAssetsCache = ComponentFactory.GetAComponent<IChromiezAssetsCache>();
        if (chromiezAssetsCache != null)
        {
            return chromiezAssetsCache.GetGameplayChromie(chromieType);
        }
        return null;
    }

    private eChromieType GetRandomChromieForSpwan()
    {
        eChromieType randomChromie = _selectedChromies[Random.Range(0, _selectedChromies.Length - 1)];
        return randomChromie;
    }


    #region Events

    private void ChromieDroppedHandler(ChromieController chromieController)
    {
        Lean.LeanPool.Despawn(chromieController.gameObject);
    }

    #endregion
}
