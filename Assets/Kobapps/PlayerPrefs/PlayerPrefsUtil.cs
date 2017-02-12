using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerPrefsUtil  {

	private static Dictionary<string, object> _cachedValues = new Dictionary<string, object>();

	private const bool CACHE_KEYS = true;

	private const string ALL_KEYS = "playerPrefsAllKeys";
	 
	public enum PlayerPrefsUtilItemType
	{
		String,
		Int, 
		Float,
		Obj,
		Bool,
	}

	private static Dictionary<string, PlayerPrefsUtilItemType> _allKeys;

	public static void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
		CacheKey(key, value, PlayerPrefsUtilItemType.Int);
	}

	public static void SetFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
		CacheKey(key, value, PlayerPrefsUtilItemType.Float);
	}

	public static void SetString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
		CacheKey(key, value, PlayerPrefsUtilItemType.String);
	}

	public static void SetObject(string key, object value)
	{
		if (value == null)
		{
			Debug.LogError("ERROR - Object value cant be null!");
			return;
		}
		string serializedObject = PlayerPrefsUtil.SerializeObjectToString(value);
		PlayerPrefs.SetString(key, serializedObject);
		CacheKey(key, value, PlayerPrefsUtilItemType.Obj);
	}

	public static void SetBool(string key, bool value)
	{

		PlayerPrefs.SetInt(key, (value) ? 1 : 0);
		CacheKey(key, value, PlayerPrefsUtilItemType.Bool);
	}

	public static int GetInt(string key)
	{
		if (PlayerPrefsUtil._cachedValues.ContainsKey(key))
		{
			if (PlayerPrefsUtil._cachedValues[key] is int)
			{
				return (int)PlayerPrefsUtil._cachedValues[key];
			}

		}
		int value = PlayerPrefs.GetInt(key);
		CacheKey(key, value, PlayerPrefsUtilItemType.Int);
		return value;
	}

	public static float GetFloat(string key)
	{
		if (PlayerPrefsUtil._cachedValues.ContainsKey(key))
		{
			if (PlayerPrefsUtil._cachedValues[key] is float)
			{
				return (float)PlayerPrefsUtil._cachedValues[key];
			}
		}
		float value = PlayerPrefs.GetFloat(key);
		CacheKey(key, value, PlayerPrefsUtilItemType.Float);
		return value;
	}

	public static string GetString(string key)
	{
		if (PlayerPrefsUtil._cachedValues.ContainsKey(key))
		{
			if (PlayerPrefsUtil._cachedValues[key] is string)
			{
				return (string)PlayerPrefsUtil._cachedValues[key];
			}
		}
		string value = PlayerPrefs.GetString(key);
		CacheKey(key, value, PlayerPrefsUtilItemType.String);
		return value;
	}

	public static object GetObject(string key)
	{
		if (PlayerPrefsUtil._cachedValues.ContainsKey(key))
		{
			return (object)PlayerPrefsUtil._cachedValues[key];
		}
		string value = PlayerPrefs.GetString(key);
		object deserializedObject = PlayerPrefsUtil.DeserializeObjectFromString(value);
		CacheKey(key, deserializedObject, PlayerPrefsUtilItemType.Obj);
		return deserializedObject;
	}

	public static bool GetBool(string key)
	{
		if (PlayerPrefsUtil._cachedValues.ContainsKey(key))
		{
			if (PlayerPrefsUtil._cachedValues[key] is string)
			{
				return (bool)PlayerPrefsUtil._cachedValues[key];
			}
		}
		int intValue = PlayerPrefs.GetInt(key);
		bool value = (intValue == 1) ? true : false;
		CacheKey(key, value, PlayerPrefsUtilItemType.Bool);
		return value;
	}

	public static bool HasKey(string key)
	{
		if (PlayerPrefsUtil._cachedValues.ContainsKey(key))
		{
			return true;
		}

		if (PlayerPrefs.HasKey(key))
		{
			return true;
		}

		return false;
	}

	public static void Delete(string key)
	{
		if (PlayerPrefsUtil.HasKey(key))
		{
			if (PlayerPrefsUtil._cachedValues.ContainsKey(key))
			{
				_cachedValues.Remove(key);
			}
			if (_allKeys.ContainsKey(key))
			{
				_allKeys.Remove(key);
				string listString = SerializeObjectToString(_allKeys);
				PlayerPrefs.SetString(ALL_KEYS, listString);
			}
			PlayerPrefs.DeleteKey(key);
		}
		else
		{
			Debug.LogWarning("Trying to delete undefiend key: " + key);
		}
	}

	public static void DeleteAll()
	{
		_allKeys.Clear();
		PlayerPrefs.DeleteAll();
		PlayerPrefsUtil._cachedValues.Clear();

	}

	public static void ClearCache()
	{
		PlayerPrefsUtil._cachedValues.Clear();
		_allKeys.Clear();
	}

	public static void ReloadAllKeys()
	{
		PlayerPrefsUtil.LoadAllKeys(true);
	}

	public static Dictionary<string, PlayerPrefsUtilItemType> GetAllKeys()
	{
		if (PlayerPrefsUtil._allKeys == null)
		{
			LoadAllKeys();
		}
		return _allKeys;
	}

	public static void DeleteKey(string key)
	{
		PlayerPrefsUtil.Delete(key);
	}

	public static void Save()
	{
		PlayerPrefs.Save();
	}

	private static void CacheKey(string key, object value, PlayerPrefsUtil.PlayerPrefsUtilItemType valueType)
	{
		if (CACHE_KEYS)
		{
			if (PlayerPrefsUtil._cachedValues.ContainsKey(key))
			{
				PlayerPrefsUtil._cachedValues[key] = value;
			}
			else
			{
				PlayerPrefsUtil._cachedValues.Add(key, value);
			}
		}

		LoadAllKeys();
		bool updatedKyes = false;
		if (!PlayerPrefsUtil._allKeys.ContainsKey(key))
		{
			PlayerPrefsUtil._allKeys.Add(key, valueType);
			updatedKyes = true;
		}
		else
		{
			if (valueType != PlayerPrefsUtil._allKeys[key])
			{
				PlayerPrefsUtil._allKeys[key] = valueType;
				updatedKyes = true;
			}
		}

		if (updatedKyes)
		{
			string listString = SerializeObjectToString(_allKeys);
			PlayerPrefs.SetString(ALL_KEYS, listString);
		}
	}

	private static string SerializeObjectToString(object obj)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		MemoryStream memoryStream = new MemoryStream();
		binaryFormatter.Serialize(memoryStream, obj);
		byte[] listBytes = memoryStream.ToArray();
		return System.Convert.ToBase64String(listBytes); 
	}

	private static object DeserializeObjectFromString(string stringData)
	{
		byte[] listByts = System.Convert.FromBase64String(stringData);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		MemoryStream memoryStream = new MemoryStream(listByts);
		return binaryFormatter.Deserialize(memoryStream);
	}

	private static void LoadAllKeys(bool forceLoad = false)
	{
		if (PlayerPrefsUtil._allKeys == null || forceLoad)
		{
			if (PlayerPrefs.HasKey(ALL_KEYS))
			{
				string allKeysData = PlayerPrefs.GetString(ALL_KEYS);
				PlayerPrefsUtil._allKeys = (Dictionary<string, PlayerPrefsUtilItemType>)DeserializeObjectFromString(allKeysData);
			}
			else
			{
				PlayerPrefsUtil._allKeys = new Dictionary<string, PlayerPrefsUtilItemType>();
			}
		}
		else
		{

		}
	}

	public static PlayerPrefsUtilItemType GetValueType(object value)
	{
		if (value is string)
		{
			return PlayerPrefsUtilItemType.String;
		}

		if (value is int)
		{
			return PlayerPrefsUtilItemType.Int;
		}

		if (value is float)
		{
			return PlayerPrefsUtilItemType.Float;
		}

		if (value is bool)
		{
			return PlayerPrefsUtilItemType.Bool;
		}

		if (value is Object)
		{
			return PlayerPrefsUtilItemType.Obj;
		}
		return PlayerPrefsUtilItemType.String; 
	}

}


