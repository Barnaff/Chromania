using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WebImageCache : MonoBehaviour , IWebImageCache {

	private Dictionary<string, Texture2D> _cachedImages;
	private Dictionary<string, string> _localFiles;


	private const string LOCAL_IMAGE_FOLDER_NAME = "images/";
	private const string LOCAL_FILES_DICTIONARY = "localFilesDictionary";

	void Start()
	{
		_cachedImages = new Dictionary<string, Texture2D>();
		_localFiles = new Dictionary<string, string>();
		LoadStoredTextures();

	}

	public bool HasImageInCache(string imageURL)
	{
		if (_cachedImages.ContainsKey(imageURL))
		{
			return true;
		}
		return false;
	}

	public void LoadImage(string imageURL, System.Action <Texture2D> completionAction, bool cacheImage = true)
	{
		if (HasImageInCache(imageURL))
		{
			Texture2D texture = _cachedImages[imageURL];
			completionAction(texture);
		}
		else
		{

			StartCoroutine(LoadImageFromWeb(imageURL, (texture)=>
			                                {
				if (texture != null && cacheImage)
				{
					if (_cachedImages.ContainsKey(imageURL))
					{
						_cachedImages[imageURL] = texture;
					}
					else
					{
						_cachedImages.Add(imageURL, texture);
						byte[] textureBytes = texture.EncodeToPNG();
						string fileName = Path.GetFileName(imageURL);
						CacheUtil.SaveToCacheAsync(textureBytes, fileName, 0, ()=>
						                      {

						});
						if (_localFiles.ContainsKey(imageURL))
						{
							_localFiles[imageURL] = fileName;
						}
						else
						{
							_localFiles.Add(imageURL, fileName);
						}
						PlayerPrefsUtil.SetObject(LOCAL_FILES_DICTIONARY, _localFiles);
					}
					 
					completionAction(texture);
				}
				else
				{
					Debug.Log("try load from disk");
					
					if (_localFiles.ContainsKey(imageURL))
					{
						CacheUtil.LoadFromCacheAsync(_localFiles[imageURL], (textureData)=>
						                             {
							if (textureData != null)
							{
								texture = new Texture2D(1, 1);
								texture.LoadImage((byte[])textureData);
								
								if (completionAction != null)
								{
									completionAction(texture);
								}
							}
						});
					}
				}

			}));
		}

	}


	IEnumerator LoadImageFromWeb(string imageURL, System.Action <Texture2D> completionAction)
	{
		WWW www = new WWW(imageURL);
		
		yield return www;

		if (www.error != null)
		{
			if (completionAction != null)
			{
				completionAction(null);
			}
		}
		else
		{
			Texture2D texture = www.texture;
			
			if (completionAction != null)
			{
				completionAction(texture);
			}
		}
	}


	private void LoadStoredTextures()
	{
		if (PlayerPrefsUtil.HasKey(LOCAL_FILES_DICTIONARY))
		{
			_localFiles = (Dictionary<string, string>)PlayerPrefsUtil.GetObject(LOCAL_FILES_DICTIONARY);
		}
	}

}
