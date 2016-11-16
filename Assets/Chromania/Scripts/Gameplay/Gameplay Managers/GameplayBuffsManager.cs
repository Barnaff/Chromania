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

    public GameplayBuffEffect CreateBuff(eBuffType buffType, float value)
    {
        GameplayBuffEffect buffEffect = new GameplayBuffEffect(buffType, value);
        if (_activeBuffs == null)
        {
            _activeBuffs = new List<GameplayBuffEffect>();
        }
        _activeBuffs.Add(buffEffect);
        return buffEffect;
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
                value *= buffEffect.Value;
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
}

[System.Serializable]
public class GameplayBuffEffect
{
    public eBuffType Type;

    public float Value;

    public GameplayBuffEffect(eBuffType type, float value)
    {
        Type = type;
        Value = value;
    }
}
