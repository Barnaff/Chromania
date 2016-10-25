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

    #endregion
}
