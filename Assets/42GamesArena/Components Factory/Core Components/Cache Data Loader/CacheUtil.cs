//#define ENABLE_CACHEUTIL_DEBUG 	
#undef ENABLE_CACHEUTIL_DEBUG

using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class CacheUtil   {

	#region Private Properties

	private const string EXTENSION_PREFIX = "gd";
	private const string PLAYER_PREFS_CACHE_KEYS = "CACHE_KEYS";
	private const string PLAYER_PREFS_VERSION_PREFIX = "CACHED_VERSION_";

	#endregion
	

	#region Public

	/// <summary>
	/// Saves to cache.
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="key">Key.</param>
	public static void SaveToCache (object data, string key, float version = 0)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream file = File.Create (CacheUtil.PathForKey(key)); 
		binaryFormatter.Serialize(file, data);
		file.Close();
		CacheUtil.AddKeyToPlayerPrefs(key);
		SetVersionToKey(key, version);
		#if (ENABLE_CACHEUTIL_DEBUG && DEBUG)
		Debug.Log("Saved file: " + key + " to path: " + CacheUtil.PathForKey(key));
		#endif
	} 

	/// <summary>
	/// Saves to cache async.
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="key">Key.</param>
	/// <param name="version">Version.</param>
	public static void SaveToCacheAsync(object data, string key, float version, System.Action completionAction)
	{
		string path = CacheUtil.PathForKey(key);
		BackgroundThreadUtil.RunBackgroundAction(()=>
		                                         {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream file = File.Create (path); 
			binaryFormatter.Serialize(file, data);
			file.Close();
		}, ()=>
		{
			CacheUtil.AddKeyToPlayerPrefs(key);
			SetVersionToKey(key, version);
			#if (ENABLE_CACHEUTIL_DEBUG && DEBUG)
			Debug.Log("Saved file: " + key + " to path: " + CacheUtil.PathForKey(key));
			#endif
			if (completionAction != null)
			{
				completionAction();
			}
		});
	}
	

	/// <summary>
	/// Loads from cache.
	/// </summary>
	/// <returns>The from cache.</returns>
	/// <param name="key">Key.</param>
	public static object LoadFromCache (string key)
	{
		if(ExistsOnCache(key)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(CacheUtil.PathForKey(key), FileMode.Open);
			object loadedData = bf.Deserialize(file);
			file.Close();

			#if (ENABLE_CACHEUTIL_DEBUG && DEBUG)
			Debug.Log("Loaded: " + key);
			#endif

			return loadedData;
		}
		#if (ENABLE_CACHEUTIL_DEBUG && DEBUG)
		Debug.LogError("ERROR - " + key + " was not found on cache!");
		#endif
		return null;
	}


	/// <summary>
	/// Loads from cache async.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="completionAction">Completion action.</param>
	public static void LoadFromCacheAsync(string key, System.Action <object> completionAction)
	{
		bool existsOnCache = ExistsOnCache(key);
		string path = CacheUtil.PathForKey(key);
		object loadedData = null;
		BackgroundThreadUtil.RunBackgroundAction(()=>
		                                         {
			if (existsOnCache)
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(path, FileMode.Open);
				loadedData = bf.Deserialize(file);
				file.Close();
				#if (ENABLE_CACHEUTIL_DEBUG && DEBUG)
				Debug.Log("Loaded: " + key);
				#endif
			}
		}, ()=>
		{
			if (completionAction != null)
			{
				completionAction(loadedData);
			}
		});
	}


	/// <summary>
	/// Existses the on cache.
	/// </summary>
	/// <returns><c>true</c>, if on cache was existsed, <c>false</c> otherwise.</returns>
	/// <param name="key">Key.</param>
	public static bool ExistsOnCache(string key)
	{
		return File.Exists(CacheUtil.PathForKey(key));
	}

	/// <summary>
	/// Versions for cached.
	/// </summary>
	/// <returns>The for cached.</returns>
	/// <param name="key">Key.</param>
	public static float VersionForCached(string key)
	{
		return CacheUtil.GetVersionForCachedKey(key);
	}

	/// <summary>
	/// Deletes from cache.
	/// </summary>
	/// <param name="key">Key.</param>
	public static void DeleteFromCache(string key)
	{
		if (File.Exists(CacheUtil.PathForKey(key)))
		{
			File.Delete(CacheUtil.PathForKey(key));
			CacheUtil.RemoveKeyFromPlayerPrefs(key);
			CacheUtil.RemoveVersionFromKey(key);
			#if (ENABLE_CACHEUTIL_DEBUG && DEBUG)
			Debug.Log("File Deleted: " + CacheUtil.PathForKey(key));
			#endif
		}
	}

	/// <summary>
	/// Clears the cache.
	/// </summary>
	public static void ClearCache()
	{
		if (PlayerPrefsUtil.HasKey(PLAYER_PREFS_CACHE_KEYS))
		{
			string keysString = PlayerPrefsUtil.GetString(PLAYER_PREFS_CACHE_KEYS);
			string[] keys = keysString.Split(","[0]);
			for (int i = 0; i< keys.Length; i++)
			{
				CacheUtil.DeleteFromCache(keys[i]);
			}
		}
	}

	#endregion


	#region Private

	private static string PathForKey(string key)
	{
		return Application.persistentDataPath + "/" + key + "." + EXTENSION_PREFIX;
	}

	private static string VersionKeyForKey(string key)
	{
		return PLAYER_PREFS_VERSION_PREFIX + key;
	}

	private static void AddKeyToPlayerPrefs(string key)
	{
		string keysString = "";
		if (PlayerPrefsUtil.HasKey(PLAYER_PREFS_CACHE_KEYS))
		{
			keysString = PlayerPrefsUtil.GetString(PLAYER_PREFS_CACHE_KEYS);
		}
		if (!keysString.Contains(key))
		{
			keysString += key + ",";
			PlayerPrefsUtil.SetString(PLAYER_PREFS_CACHE_KEYS, keysString);
		}
	}

	private static void RemoveKeyFromPlayerPrefs(string key)
	{
		if (PlayerPrefsUtil.HasKey(PLAYER_PREFS_CACHE_KEYS))
		{
			string keysString = PlayerPrefsUtil.GetString(PLAYER_PREFS_CACHE_KEYS);
			if (keysString.Contains(key))
			{
				keysString.Remove(keysString.IndexOf(key, 0, System.StringComparison.Ordinal), key.Length);
			}
		}
	}

	private static void SetVersionToKey(string key, float version)
	{
		PlayerPrefsUtil.SetFloat(VersionKeyForKey(key), version);
	}

	private static float GetVersionForCachedKey(string key)
	{
		if (PlayerPrefsUtil.HasKey(VersionKeyForKey(key)))
		{
			float version = PlayerPrefsUtil.GetFloat(VersionKeyForKey(key));
			return version;
		}
		return 0;
	}

	private static void RemoveVersionFromKey(string key)
	{
		if (PlayerPrefsUtil.HasKey(VersionKeyForKey(key)))
		{
			PlayerPrefsUtil.DeleteKey(VersionKeyForKey(key));
		}
	}

	#endregion


}
