using UnityEngine;
using System.Collections;

public static class ePowerups  {

	public enum Active
    {
        None,
        AddLife,
        AddTime,
        DoubleScore,
    }

    public enum Passive
    {
        None,
        AddLifeAtStart,
        AddLifeLostAtStart,
        AddTimeAtStart,

    }
}
