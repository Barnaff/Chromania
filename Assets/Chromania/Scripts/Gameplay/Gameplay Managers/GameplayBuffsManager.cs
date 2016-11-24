using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayBuffsManager : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private List<GameplayBuffEffect> _activeBuffs;

    #endregion

    #region Singleton Lifesycle

    private static GameplayBuffsManager _instance;

    public static GameplayBuffsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("ERROR - GameplayBuffsManager is not in the scene!");
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    void OnDestory()
    {
        _instance = null;
    }

    public static float GetValue(eBuffType buffType)
    {
        return Instance.GetCurrentValue(buffType);
    }

    #endregion


    #region Public

    public GameplayBuffEffect CreateBuff(eBuffType buffType, float value, eBuffMetod method)
    {
        GameplayBuffEffect buffEffect = new GameplayBuffEffect(buffType, value, method);
        if (_activeBuffs == null)
        {
            _activeBuffs = new List<GameplayBuffEffect>();
        }
        _activeBuffs.Add(buffEffect);
        return buffEffect;
    }

    public GameplayBuffEffect CreateBuff(GameplayBuffEffect buffEffect)
    {
        return CreateBuff(buffEffect.Type, buffEffect.Value, buffEffect.Method);
    }

    public void RemoveBuff(GameplayBuffEffect buffEffect)
    {
        if (_activeBuffs != null && _activeBuffs.Contains(buffEffect))
        {
            _activeBuffs.Remove(buffEffect);
        }
    }

    public float GetCurrentValue(eBuffType buffType)
    {
        float value = 1;
        foreach (GameplayBuffEffect buffEffect in _activeBuffs)
        {
            if (buffEffect.Type == buffType)
            {
                switch (buffEffect.Method)
                {
                    case eBuffMetod.Multiplier:
                        {
                            value *= buffEffect.Value;
                            break;
                        }
                    case eBuffMetod.Add:
                        {
                            value += buffEffect.Value;
                            break;
                        }
                }
            }
        }
        return value;
    }

    #endregion

}

public enum eBuffType
{
    ScoreMultiplier,
    ComboMultiplier,
    ColorZoneSizeMultiplier,
    PowerupSpwanChanceMultiplier,
    PowerupEffectMultiplier,
    CurrencyCollectedMultiplier,
}

public enum eBuffMetod
{
    Multiplier,
    Add,
}

[System.Serializable]
public class GameplayBuffEffect
{
    public eBuffType Type;

    public float Value;

    public eBuffMetod Method;

    public GameplayBuffEffect(eBuffType type, float value, eBuffMetod method)
    {
        Type = type;
        Value = value;
        Method = method;
    }

    public override string ToString()
    {
        return string.Format("[GameplayBuffEffect] Type: {0}, Value: {1}, Method: {2}", Type, Value, Method);
    }
}
