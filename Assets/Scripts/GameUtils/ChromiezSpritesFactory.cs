using UnityEngine;
using System.Collections;

public class ChromiezSpritesFactory : MonoBehaviour  {

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

	private static ChromiezSpritesFactory _instance;
	

	public static ChromiezSpritesFactory Instance()
	{
	
		return ChromiezSpritesFactory._instance;
	}

	void Awake()
	{
		ChromiezSpritesFactory._instance = this;
	}
	

	public static Sprite GetChromieSprite(ColorType colorType)
	{
		Sprite chromieSprite = null;
		switch (colorType)
		{
			case ColorType.Red:
			{
			chromieSprite = ChromiezSpritesFactory.Instance().Red;
			break;
			}
			case ColorType.Blue:
			{
			chromieSprite = ChromiezSpritesFactory.Instance().Blue;
			break;
			}
			case ColorType.Green:
			{
			chromieSprite = ChromiezSpritesFactory.Instance().Green;
			break;
			}
			case ColorType.Jester:
			{
			chromieSprite = ChromiezSpritesFactory.Instance().Jester;
			break;
			}
			case ColorType.Ninja:
			{
			chromieSprite = ChromiezSpritesFactory.Instance().Ninja;
			break;
			}
			case ColorType.Orange:
			{
			chromieSprite = ChromiezSpritesFactory.Instance().Orange;
			break;
			}
			case ColorType.Purple:
			{
			chromieSprite = ChromiezSpritesFactory.Instance().Purple;
			break;
			}
			case ColorType.Robot:
			{
			chromieSprite = ChromiezSpritesFactory.Instance().Robot;
			break;
			}
			case ColorType.Yellow:
			{
			chromieSprite = ChromiezSpritesFactory.Instance().Yellow;
			break;
			}
			case ColorType.Yeti:
			{
			chromieSprite = ChromiezSpritesFactory.Instance().Yeti;
			break;
			}
			case ColorType.Zombie:
			{
			chromieSprite = ChromiezSpritesFactory.Instance().Zombie;
			break;
			}
		}

		return chromieSprite;
	}
}
