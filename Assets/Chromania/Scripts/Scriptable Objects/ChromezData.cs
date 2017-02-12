using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChromezData : Kobapps.ScriptableSingleton<ChromezData> {

    #region Public Properties

    public List<ChromieDefenition> Chromiez;

    #endregion


    #region Public

    public ChromieDefenition GetChromie(eChromieType chromieType)
    {
        for (int i = 0; i < Chromiez.Count; i++)
        {
            if (Chromiez[i].ChromieColor == chromieType)
            {
                return Chromiez[i];
            }
        }
        return null;
    }

    public List<ChromieDefenition> ChromiezForIds(int[] ids)
    {
        List<ChromieDefenition> chromiezForIds = new List<ChromieDefenition>();
        for (int i=0; i< ids.Length; i++)
        {
            foreach (ChromieDefenition chromieDefenition in Chromiez)
            {
                if (chromieDefenition.ChromieID == ids[i])
                {
                    chromiezForIds.Add(chromieDefenition);
                }
            }
        }
        return chromiezForIds;
    }

    #endregion
}
