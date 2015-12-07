using UnityEngine;
using System.Collections;

public interface IWebImageCache  
{
	bool HasImageInCache(string imageURL);

	void LoadImage(string imageURL, System.Action <Texture2D> completionAction, bool cachedImage = true);
}
