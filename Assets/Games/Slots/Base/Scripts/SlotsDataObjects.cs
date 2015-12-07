using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class SlotsReelInfo
{
	public int ReelID;
	public GameObject ReelContainer;
	public int[] ReelIcons;

}

[System.Serializable]
public class SlotsDefenition
{
	[Range(0,5)]
	public int NumberOfReels;

	[Range(0,100)]
	public int TilesPerReel;

	[Range(0,20)]
	public int TilesPerReelDifirance;

	//public Shader LineShader;

	public float[] SlotsBetAmounts;

	public SlotsIconDefenition[] IconsDefenitions;

	public SlotsLineDefenition[] Lines;



}

[System.Serializable]
public class SlotsIconDefenition
{
	public Sprite IconSprite;

	[System.Serializable]
	public enum IconAnimationType
	{
		None,
		Wiggle, 
		Punch, 
		SpinX, 
		SpinY,
		SpinZ,
	}

	public IconAnimationType IconAnimation;

	[System.Serializable]
	public enum SlotsIconRarityType
	{
		Common,
		Uncommon,
		Rare, 
		Mythic,
	}

	public SlotsIconRarityType IconRarity;

	[System.Serializable]
	public enum SlotsIconBehaviorType
	{
		Normal,
		Joker,
		Bonus,
		Scatter,
	}

	public SlotsIconBehaviorType IconBehaviorType;

	public float BetMultiplierFor3 = 1.5f;
	public float BetMultiplierFor4 = 2.0f;
	public float BetMultiplierFor5 = 4.0f;

	public string IconSpriteName;
}

[System.Serializable]
public class SlotsLineDefenition
{

	public enum LineAnchorPositionType
	{
		Top,
		Middle,
		Bottom,
	}

	public LineAnchorPositionType[] AnchorPositions;

	public Color LineColor;

}

[System.Serializable]
public class SlotsWinningsInfo
{
	public SlotsIconDefenition Icon = null;

	public int LineIndex = 0;

	public int Count = 0;

	public List<SlotsTileController> Tiles = new List<SlotsTileController>();

	public int JokerCount = 0;

	public bool isWinning = false;
}

[System.Serializable]
public class SlotsSimulationResults
{

	public int SpinsCounted = 0;

	public int CoinsBalance = 0;

	public int PayAmount = 0;

	public int WinAmount = 0;

	public Dictionary<string, SlotsSimulationIconInfo> IconsInfo = new Dictionary<string, SlotsSimulationIconInfo>();

	public int NumberOfScatters = 0;

	public int SavedByScatter = 0;

	public int NumberOfWildCompletions = 0;

	public int WinCount = 0;

	public int Tripples = 0;

	public int Forths = 0;

	public int Fifths = 0;

	public int JokerCount = 0;

	public int MaxWin = 0;
}

[System.Serializable]
public class SlotsSimulationIconInfo
{
	public int Count = 0;

	public int Trippels = 0;

	public int Forths = 0;

	public int Fifths = 0;

	public int TotalWin = 0;

	public float Percentage = 0;
}


public class SlotsIconDefenitionSurrogate : ISerializationSurrogate 
{	
	public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context) 
	{
		SlotsIconDefenition iconDefenition = (SlotsIconDefenition) obj;
		info.AddValue("IconAnimation", iconDefenition.IconAnimation);
		info.AddValue("IconRarity", iconDefenition.IconRarity);
		info.AddValue("IconBehaviorType", iconDefenition.IconBehaviorType);
		info.AddValue("BetMultiplierFor3", iconDefenition.BetMultiplierFor3);
		info.AddValue("BetMultiplierFor4", iconDefenition.BetMultiplierFor4);
		info.AddValue("BetMultiplierFor5", iconDefenition.BetMultiplierFor5);
		string iconSpriteName = iconDefenition.IconSpriteName;
		if (iconDefenition.IconSprite != null)
		{
			iconSpriteName = iconDefenition.IconSprite.name;
		}
		info.AddValue("IconSpriteName", iconSpriteName);
	}
	
	public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) 
	{
		SlotsIconDefenition iconDefenition = (SlotsIconDefenition) obj;
		iconDefenition.IconAnimation = (SlotsIconDefenition.IconAnimationType)info.GetValue("IconAnimation", typeof(SlotsIconDefenition.IconAnimationType));
		iconDefenition.IconBehaviorType = (SlotsIconDefenition.SlotsIconBehaviorType)info.GetValue("IconBehaviorType", typeof(SlotsIconDefenition.SlotsIconBehaviorType));
		iconDefenition.IconRarity = (SlotsIconDefenition.SlotsIconRarityType)info.GetValue("IconRarity", typeof(SlotsIconDefenition.SlotsIconRarityType));
		iconDefenition.BetMultiplierFor3 = (float)info.GetValue("BetMultiplierFor3", typeof(float));
		iconDefenition.BetMultiplierFor4 = (float)info.GetValue("BetMultiplierFor4", typeof(float));
		iconDefenition.BetMultiplierFor5 = (float)info.GetValue("BetMultiplierFor5", typeof(float));
		iconDefenition.IconSpriteName = (string)info.GetValue("IconSpriteName", typeof(string));
		obj = iconDefenition;
		return obj; 
	}
}