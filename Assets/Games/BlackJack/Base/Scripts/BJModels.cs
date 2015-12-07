using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BJGameModel
{
    public BJPlayerModel Player = new BJPlayerModel();
    public BJPlayerModel Dealer = new BJPlayerModel();

    public List<BJCardModel> Deck = new List<BJCardModel>();
    public List<BJCardModel> DiscardsDekc = new List<BJCardModel>();
}

[System.Serializable]
public class BJPlayerModel
{
    public BJHandModel PrimaryHand = new BJHandModel();
	public BJHandModel SecondaryHand = new BJHandModel();

}

[System.Serializable]
public class BJHandModel
{
    public List<BJCardModel> Cards = new List<BJCardModel>();
    public bool Stand;

    public int Value
    {
        get
        {
            int lowValue = 0;
            int highValue = 0;
			bool haveAce = false;

            foreach (BJCardModel cardModel in Cards)
            {
                if (cardModel.IsFlipped)
                {
                    if (cardModel.CardValue == 11)
                    {
                        lowValue += 1;
						if (!haveAce)
						{
							highValue += cardModel.CardValue;
							haveAce = true;
						}
						else
						{
							highValue += 1;
						}
                    }
                    else
                    {
                        lowValue += cardModel.CardValue;
                        highValue += cardModel.CardValue;
                    }
                }
            }
            int finalValue = highValue;
            if (highValue > 21)
            {
                finalValue = lowValue;
            }
            return finalValue;
        }
    }
}

[System.Serializable]
public class BJCardModel
{
    public int CardValue;
    public int CardId;

    public enum eBJCardType
    {
        Diamonds,
        Clover,
        Heart,
        Spade,
    }

    public eBJCardType CardType;
    public bool IsFlipped;

    public BJCardModel(int cardId)
    {
        CardId = cardId;
        CardValue = 2 + CardId % 13;
        if (CardValue == 14)
        {
            CardValue = 11;
        }
        else if (CardValue > 10)
        {
            CardValue = 10;
        }
        CardType = (eBJCardType)(CardId / 13);
    }
}

