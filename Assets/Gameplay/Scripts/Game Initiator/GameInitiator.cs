using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInitiator  {


    private List<GameObject> _chromiezPreviews;

    public static void StartGameInit(ColorType[] selectedColors, GameModeType gameMode, System.Action completionAction)
    {
        GameInitiator gameIntro = new GameInitiator();
        gameIntro.InitGame(selectedColors, gameMode, completionAction);
    }

    #region private

    private void InitGame(ColorType[] selectedColors, GameModeType gameMode, System.Action completionAction)
    {
        _chromiezPreviews = new List<GameObject>();
        ColorZonesManager colorZonesManager = GameObject.FindObjectOfType<ColorZonesManager>() as ColorZonesManager;
        if (colorZonesManager == null)
        {
            Debug.LogError("ERROR - Cannot find ColorZonesManager");
        }

        int indexCount = 0;
        foreach (ColorType color in selectedColors)
        {
            DisplayChomieEnterAnimation(color, indexCount, () =>
            {
                indexCount--;
                if (indexCount == 0)
                {
                    if (completionAction != null)
                    {
                        completionAction();
                    }
                }
            });
            indexCount++;
        }
    }

  
    private void DisplayChomieEnterAnimation(ColorType color, int index, System.Action completionAction)
    {
        GameObject chromiePreview = CreateChromie(color);
        chromiePreview.transform.position = new Vector3(-3.0f + (index * 2.0f), 0, 0);
        chromiePreview.GetComponent<Rigidbody2D>().isKinematic = true;
        _chromiezPreviews.Add(chromiePreview);
        ChromieInitiatorController chromieInitiator = chromiePreview.AddComponent<ChromieInitiatorController>() as ChromieInitiatorController;
        if (chromieInitiator != null)
        {
            chromieInitiator.DisplayChromieIntro(index, 1.0f + index, () =>
            {
                DisplayColorZoneEnter(color, index,  () =>
                {
                    DisplayPassivePowerupActivation(color, () =>
                    {
                        completionAction();
                    });
                   
                });
            });
        }
    }

    private void DisplayColorZoneEnter(ColorType color, int index, System.Action completionAction)
    {
        ColorZonesManager colorZoneManager = GameObject.FindObjectOfType<ColorZonesManager>() as ColorZonesManager;
        if (colorZoneManager != null)
        {
            colorZoneManager.CreateColorZoneObject(color, index, true, ()=>
            {
                completionAction();
            });
        }
       
    }

    private void DisplayPassivePowerupActivation(ColorType color, System.Action completionAction)
    {
        PowerupsManager powerupsManager = GameObject.FindObjectOfType<PowerupsManager>() as PowerupsManager;
        if (powerupsManager != null)
        {
            GameDataLoadermanager dataLoaderManager = GameObject.FindObjectOfType<GameDataLoadermanager>() as GameDataLoadermanager;
            if (dataLoaderManager != null)
            {
                ChromieDataItem chromieData = dataLoaderManager.GetGameData().GetChromie(color);
                powerupsManager.ActivatePssive(chromieData);
            }
        }
        completionAction();
    }

    private GameObject CreateChromie(ColorType color)
    {
        GameObject chromie = null;
        SpwanManager spwanManager = GameObject.FindObjectOfType<SpwanManager>() as SpwanManager;
        if (spwanManager != null)
        {
            foreach (GameObject chromiePrefab in spwanManager.ChromiezPrefabs)
            {
                if (chromiePrefab.GetComponent<ChromieController>().ChromieType == color)
                {
                    chromie = GameObject.Instantiate(chromiePrefab) as GameObject;
                    return chromie;
                }
            }
        }

        return chromie;
    }

    #endregion
}
