using UnityEngine;
using System.Collections;

public class ChromiezSpritesUtils : MonoBehaviour  {

	public Sprite Red;
	public Sprite Blue;
	public Sprite Green;
	public Sprite Yellow;
	public Sprite Purple;
	public Sprite Jester;
	public Sprite Orange;
	public Sprite Yeti;
	public Sprite Ninja;
	public Sprite Robot;
	public Sprite Zombie;

	private static ChromiezSpritesUtils _instance;
	

	public static ChromiezSpritesUtils Instance()
	{
		if (ChromiezSpritesUtils._instance == null)
		{
			GameObject chromiezSpriteContainer = new GameObject();
			ChromiezSpritesUtils._instance = chromiezSpriteContainer.AddComponent<ChromiezSpritesUtils>() as ChromiezSpritesUtils;
			DontDestroyOnLoad(chromiezSpriteContainer);
		}

		return ChromiezSpritesUtils._instance;
	}
	

	public static Sprite GetChromieSprite(ColorType colorType)
	{
		Sprite chromieSprite = null;
		switch (colorType)
		{
			case ColorType.Red:
			{
				chromieSprite = ChromiezSpritesUtils.Instance().Red;
				break;
			}
			case ColorType.Blue:
			{
				chromieSprite = ChromiezSpritesUtils.Instance().Blue;
				break;
			}
			case ColorType.Green:
			{
				chromieSprite = ChromiezSpritesUtils.Instance().Green;
				break;
			}
			case ColorType.Jester:
			{
				chromieSprite = ChromiezSpritesUtils.Instance().Jester;
				break;
			}
			case ColorType.Ninja:
			{
				chromieSprite = ChromiezSpritesUtils.Instance().Ninja;
				break;
			}
			case ColorType.Orange:
			{
				chromieSprite = ChromiezSpritesUtils.Instance().Orange;
				break;
			}
			case ColorType.Purple:
			{
				chromieSprite = ChromiezSpritesUtils.Instance().Purple;
				break;
			}
			case ColorType.Robot:
			{
				chromieSprite = ChromiezSpritesUtils.Instance().Robot;
				break;
			}
			case ColorType.Yellow:
			{
				chromieSprite = ChromiezSpritesUtils.Instance().Yellow;
				break;
			}
			case ColorType.Yeti:
			{
				chromieSprite = ChromiezSpritesUtils.Instance().Yeti;
				break;
			}
			case ColorType.Zombie:
			{
				chromieSprite = ChromiezSpritesUtils.Instance().Zombie;
				break;
			}
		}

		return chromieSprite;
	}
}
