using UnityEngine;
using System.Collections;

[System.Serializable]
public class AdTileDataModel : TileDataModelAbstract {

	[SerializeField]
	public AdDataModel Ad;

	public string ImageForTileSize(eTileSize tileSize)
	{
		string imageURL = "";
		switch(tileSize)
		{
		case eTileSize.TILESIZE1:
		{
			imageURL = Ad.AdImageForSize(AdDataModel.eAdImageSizeType.SIZE1);
			break;
		}
		case eTileSize.TILESIZE2:
		{
			imageURL = Ad.AdImageForSize(AdDataModel.eAdImageSizeType.SIZE2);
			break;
		}
		case eTileSize.TILESIZE3:
		{
			imageURL = Ad.AdImageForSize(AdDataModel.eAdImageSizeType.SIZE3);
			break;
		}
		default:
		{
			break;
		}
		}

		if (string.IsNullOrEmpty(imageURL))
		{
			Debug.LogError("ERROR - Missing ad image for tile size: " + tileSize);
		}

		return imageURL;
	}
}
