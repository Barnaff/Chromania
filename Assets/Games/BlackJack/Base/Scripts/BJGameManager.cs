using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BJGameManager : GameplayBase
{


    #region Private proeprties

    [SerializeField]
    private BJCardController _cardPrefab;

    [SerializeField]
    private Sprite[] _cardsSprites;

    [SerializeField]
    private GameObject _playerPile1Mark;

    [SerializeField]
    private GameObject _playerPile2Mark;

    [SerializeField]
    private GameObject _dealerPileMark;

    [SerializeField]
    private BJPopupsController _popupsController;

    [SerializeField]
    private BJButtonsController _buttonsController;

    [SerializeField]
    private Text _betLabel;

    [SerializeField]
    private int _numberOfDecks = 1;

    [SerializeField]
    private Text _playerValueLabel;

    [SerializeField]
    private Text _dealerValueLabel;

    [SerializeField]
    private int[] _betPerks;

    [SerializeField]
    private float _cardMoveAnimationDuration = 0.5f;

	[SerializeField]
	private bool _enableDebug;

    [SerializeField]
    private BJGameModel _gameModel;

    private List<BJCardController> _cardsPool;
    private Dictionary<BJCardModel, BJCardController> _activeCards;
	private int _cardsCount = 100;
	private bool _isSplitActive = false;
	private int _splitCount = 0;
	private bool _isDoubleActive = false;
	private bool _roundStarted = false;
	private bool _roundFinished = false;
	private int _currentBetIndex = 0;
	private int _currentBetAmount = 0;

    #endregion


    #region GameplayBase Subclassing

    protected override void Init()
    {
       

        _cardsPool = new List<BJCardController>();
        _activeCards = new Dictionary<BJCardModel, BJCardController>();

        _buttonsController.Init();
        _buttonsController.OnDealButtonAction += HandleOnDealButtonAction;
        _buttonsController.OnHitButtonAction += HandleOnHitButtonAction;
        _buttonsController.OnStandButtonAction += HandleOnStandButtonAction;
        _buttonsController.OnSplitButtonAction += HandleOnSplitButtonAction;
        _buttonsController.OnDoubleButtonAction += HandleOnDoubleButtonAction;
        _buttonsController.OnPlusButtonAction += HandleOnPlusButtonAction;
        _buttonsController.OnMinusButtonAction += HandleOnMinusButtonAction;
        _popupsController.Init();

		if (_enableDebug)
		{
			Debug.Log("<color=blue> Game Initialized </color>");
		}
    }

    protected override void GameStarted(string gameVariables)
    {
		Debug.Log("GAME STARTED: " + gameVariables);

        _gameModel = new BJGameModel();
        for (int decks = 0; decks < _numberOfDecks; decks++)
        {
            for (int i = 0; i < _cardsSprites.Length; i++)
            {
                BJCardModel card = new BJCardModel(i);
                _gameModel.Deck.Add(card);
            }
        }

		if (!string.IsNullOrEmpty(gameVariables))
		{
			string[] stringArray = gameVariables.Split(","[0]);
			_betPerks = new int[stringArray.Length];
			for (int i=0; i< stringArray.Length; i++)
			{
				_betPerks[i] = int.Parse(stringArray[i]);
			}
		}

        _buttonsController.EnableDealButton();


		if (PlayerPrefsUtil.HasKey("_currentBetIndex"))
		{
			_currentBetIndex = PlayerPrefsUtil.GetInt("_currentBetIndex");
		}
		if (_currentBetIndex < 0 || _currentBetIndex > _betPerks.Length)
		{
			_currentBetIndex = 0;
		}
		_betLabel.text = _betPerks[_currentBetIndex].ToString();

		if (_enableDebug)
		{
			Debug.Log("<color=blue> GameStarted </color>");
		}

	}
	
	#endregion
	
	
	#region Gameplay

    IEnumerator NewRound()
    {
		if (_enableDebug)
		{
			Debug.Log("<color=blue> New Round </color>");
		}

		_roundFinished = false;
		_roundStarted = false;

		if (_gameModel.Player.PrimaryHand.Cards.Count > 0 || _gameModel.Dealer.PrimaryHand.Cards.Count > 0)
		{
			yield return StartCoroutine(ClearTable());
		}

		_isSplitActive = false;
		_splitCount = 0;
		_isDoubleActive = false;

		_buttonsController.EnableGameActionsButtons();
		_buttonsController.LockGameplayButtons();

		UpdateScoreCounts();

        yield return StartCoroutine(DealCardToPlayer());

		yield return StartCoroutine(DealCardToDealer());

		yield return StartCoroutine(DealCardToPlayer());

		if (!_roundFinished)
		{
			yield return StartCoroutine(DealCardToDealer(false));
			
			yield return null;
			_buttonsController.EnableGameActionsButtons();
			
			_buttonsController.EnableDoubleButton();
			
			if (_gameModel.Player.PrimaryHand.Cards.Count == 2)
			{
				if (_gameModel.Player.PrimaryHand.Cards[0].CardValue == _gameModel.Player.PrimaryHand.Cards[1].CardValue)
				{
					_buttonsController.EnableSplitButton();
				}
			}
		}
    }

	IEnumerator DealCardToPlayer(BJHandModel hand = null)
    {
		if (!_roundStarted)
		{
			if (_enableDebug)
			{
				Debug.Log("<color=blue> Skipping deal card to player </color>");
			}
			yield return null;
		}
		if (_enableDebug)
		{
			Debug.Log("<color=blue> Deal Card To Player </color>");
		}

		_buttonsController.LockGameplayButtons();

        BJCardModel cardModel = DrawCard();

        BJCardController cardController = GetCardController(cardModel);

		if (hand == null)
		{
			hand = _gameModel.Player.PrimaryHand;
		}
		else
		{
			StartCoroutine(cardController.SetCardActiveState(false));
		}

		yield return StartCoroutine(MoveCardToHand(cardModel, hand));

		PlaySoundFX("Card_Flip");

        yield return StartCoroutine(cardController.Flip(true));

        UpdateScoreCounts();

		if (hand == _gameModel.Player.PrimaryHand)
		{
			if (_gameModel.Player.PrimaryHand.Value >= 21)
			{
				yield return StartCoroutine(FinishRound());
			}
			else
			{
				if (_gameModel.Player.PrimaryHand.Cards.Count >= 2)
				{
					_buttonsController.EnableGameActionsButtons();
				}
				
				if (_isDoubleActive)
				{
					_isDoubleActive = false;
					yield return StartCoroutine(PlayStand());
				}
			}
		}
    }

    IEnumerator DealCardToDealer(bool isFlipped = true)
    {
		if (!_roundStarted)
		{
			if (_enableDebug)
			{
				Debug.Log("<color=blue> Skipping deal card to dealer </color>");
			}
			yield return null;
		}

		if (_enableDebug)
		{
			Debug.Log("<color=blue> Deal Card To Dealer </color>");
		}

        BJCardModel cardModel = DrawCard();

        if (isFlipped)
        {
			BJCardController cardController = GetCardController(cardModel);
			yield return StartCoroutine(cardController.Flip(false));
        }

        yield return StartCoroutine(MoveCardToHand(cardModel, _gameModel.Dealer.PrimaryHand));
        UpdateScoreCounts();
    }

    IEnumerator FinishRound()
    {
		if (_enableDebug)
		{
			Debug.Log("<color=blue> Finish Round </color>");
		}
		_roundFinished = true;
		if (_isSplitActive && _splitCount == 0)
		{
			yield return StartCoroutine(SwitchHands());
			_splitCount++;
			yield break;
		}

		yield return StartCoroutine(FlipAllCards());
		
		int dealerScore = _gameModel.Dealer.PrimaryHand.Value;
		int playerScore = _gameModel.Player.PrimaryHand.Value;


		if ((playerScore == 21 || playerScore > dealerScore || dealerScore > 21) && playerScore <= 21)
		{
			// player win
			if (Random.Range(0,100) > 50)
			{
				PlaySoundFX("Win1");
			}
			else
			{
				PlaySoundFX("Win2");
			}

			int winAmount = _currentBetAmount;
			if (playerScore == 21)
			{
				if (_gameModel.Player.PrimaryHand.Cards.Count == 2)
				{
					winAmount = _currentBetAmount + (int)(_currentBetAmount * 1.5f);
				}
				else
				{
					winAmount *= 2;
				}
			}
			else
			{
				winAmount *= 2;
			}

			AddCoins(winAmount);

			yield return StartCoroutine(_popupsController.DisplayYouWin(winAmount));
		}
		else if ((dealerScore == 21 || dealerScore > playerScore || playerScore > 21) && dealerScore <= 21)
		{
			// dealer win
			yield return StartCoroutine(_popupsController.DisplayGameplayMessage("Unlucky..."));
		}
		else if (playerScore == dealerScore)
		{
			// tie
			AddCoins(_currentBetAmount);
			yield return StartCoroutine(_popupsController.DisplayGameplayMessage("Push"));
		}

		
		yield return null;

		if (_isSplitActive)
		{
			Debug.Log("switch hands after finish round");

			yield return StartCoroutine(SwitchHands());
			_isSplitActive = false;

			yield return StartCoroutine(FinishRound());

			yield break;
		}
		_roundStarted = false;
		yield return StartCoroutine(ClearTable());

		_buttonsController.EnableDealButton();
    }

    IEnumerator FlipAllCards()
    {
		if (_enableDebug)
		{
			Debug.Log("<color=blue> Flip All Cards </color>");
		}

        foreach (BJCardController card in _activeCards.Values)
		{
			if (!card.IsFlipped)
			{
				PlaySoundFX("Card_Flip");
				yield return StartCoroutine(card.Flip(true));
			}
			UpdateScoreCounts();
        }
    }

    IEnumerator PlayDealer()
    {
		if (_enableDebug)
		{
			Debug.Log("<color=blue> Play Dealer </color>");
		}

		_buttonsController.LockGameplayButtons();

        yield return StartCoroutine(FlipAllCards());

        while (_gameModel.Dealer.PrimaryHand.Value < 17)
        {
            yield return StartCoroutine(DealCardToDealer(true)); 
        }

		Debug.Log("play dealer called for finish round");
        yield return StartCoroutine(FinishRound());
    }

	IEnumerator PlayStand()
	{
		if (_isSplitActive)
		{
			if (_splitCount == 0)
			{
				yield return StartCoroutine(SwitchHands());
				_splitCount++;
				yield break;
			}
		}

		yield return StartCoroutine(PlayDealer());
	}

	IEnumerator ClearTable()
	{
		if (_enableDebug)
		{
			Debug.Log("<color=blue> Clear table </color>");
		}

		PlaySoundFX("Card_Shuffle");

		_betLabel.text = _betPerks[_currentBetIndex].ToString();

		List<BJCardModel> cardsToRemove = new List<BJCardModel>();
		foreach (BJCardModel cardModel in _activeCards.Keys)
		{
			cardsToRemove.Add(cardModel);
			StartCoroutine(MoveCardToDeck(GetCardController(cardModel)));
			yield return new WaitForSeconds(0.1f);
		}

		foreach (BJCardModel cardModel in cardsToRemove)
		{
			RemoveCard(cardModel);
		}

		DiscardPlayersHands();

		yield return null;
	}


	IEnumerator PlayDouble()
	{
		if (_enableDebug)
		{
			Debug.Log("<color=blue> Play Double </color>");
		}

		_buttonsController.LockGameplayButtons();

		_isDoubleActive = true;

		yield return StartCoroutine(DealCardToPlayer());

	}

	IEnumerator PlaySplit()
	{
		_isSplitActive = true;
		_buttonsController.LockGameplayButtons();

		BJCardModel secondCard = _gameModel.Player.PrimaryHand.Cards[1];
		//BJCardController secondCardController = GetCardController(secondCard);

		_gameModel.Player.PrimaryHand.Cards.Remove(secondCard);

		yield return StartCoroutine(MoveCardToHand(secondCard, _gameModel.Player.SecondaryHand));

		UpdateScoreCounts();

		yield return StartCoroutine(SortPlayerHands());

		yield return null;


		yield return StartCoroutine(DealCardToPlayer(_gameModel.Player.SecondaryHand));
		yield return StartCoroutine(DealCardToPlayer());

	

		yield return null;

		_buttonsController.EnableGameActionsButtons();
	}

	IEnumerator SortPlayerHands()
	{
		foreach (BJCardModel cardModel in _gameModel.Player.PrimaryHand.Cards)
		{
			BJCardController cardController = GetCardController(cardModel);
			StartCoroutine(MoveCardToHand(cardModel, _gameModel.Player.PrimaryHand));
			yield return StartCoroutine(cardController.SetCardActiveState(true));
		}

		foreach (BJCardModel cardModel in _gameModel.Player.SecondaryHand.Cards)
		{
			BJCardController cardController = GetCardController(cardModel);
			StartCoroutine(MoveCardToHand(cardModel, _gameModel.Player.SecondaryHand));
			yield return StartCoroutine(cardController.SetCardActiveState(false));
		}

		yield return null;
	}

	IEnumerator SwitchHands()
	{
		BJHandModel tmpHand = _gameModel.Player.SecondaryHand;

		_gameModel.Player.SecondaryHand = _gameModel.Player.PrimaryHand;
		_gameModel.Player.PrimaryHand = tmpHand;

		yield return StartCoroutine(SortPlayerHands());

		yield return null;

		if (_splitCount == 0)
		{
			_buttonsController.EnableGameActionsButtons();
		}

		UpdateScoreCounts();
	}

    #endregion



    #region Private

    IEnumerator MoveCardToHand(BJCardModel cardModel, BJHandModel hand)
    {
		if (_gameModel.Deck.Contains(cardModel))
		{
        	_gameModel.Deck.Remove(cardModel);
		}
		if (!hand.Cards.Contains(cardModel))
		{
			hand.Cards.Add(cardModel);
		}
        

       // BJCardController cardController = GetCardController(cardModel);
       // cardController.transform.position = _cardPrefab.transform.position;

        GameObject handContainer = null;
        if (hand == _gameModel.Player.PrimaryHand)
        {
            handContainer = _playerPile1Mark;
        }
        if (hand == _gameModel.Dealer.PrimaryHand)
        {
            handContainer = _dealerPileMark;
        }
		if (hand == _gameModel.Player.SecondaryHand)
		{
			handContainer = _playerPile2Mark;
		}

        Vector3 position = Camera.main.WorldToScreenPoint(handContainer.transform.position);
        position.z = 10;
        position = Camera.main.ScreenToWorldPoint(position);

        for (int i=0; i< hand.Cards.Count; i++)
        {
            BJCardController cardInHand = GetCardController(hand.Cards[i]);
            Vector3 positionInHand = position;
            positionInHand.x += 0.7f * i - (0.7f * hand.Cards.Count * 0.2f);
			iTween.MoveTo(cardInHand.gameObject, iTween.Hash("time", _cardMoveAnimationDuration, "position", positionInHand));
        }

        yield return new WaitForSeconds(_cardMoveAnimationDuration);

    }

    private BJCardModel DrawCard()
    {
        if (_gameModel.Deck.Count == 0)
        {
            _gameModel.Deck = _gameModel.DiscardsDekc;
            _gameModel.DiscardsDekc = new List<BJCardModel>();
        }

        BJCardModel card = _gameModel.Deck[Random.Range(0,_gameModel.Deck.Count)];
		card.IsFlipped = false;
        _cardsCount++;
        return card;
    }

    private BJCardController GetCardController(BJCardModel cardModel)
    {
        if (_activeCards.ContainsKey(cardModel))
        {
            return _activeCards[cardModel];
        }

        BJCardController card = null;
        if (_cardsPool.Count > 0)
        {
            card = _cardsPool[0];
            _cardsPool.Remove(card);
        }

        if (card == null)
        {
            card = Instantiate(_cardPrefab) as BJCardController;
        }
		card.gameObject.name = cardModel.CardType + ":" +  cardModel.CardValue;
		card.SetCardModel(cardModel, _cardsSprites[cardModel.CardId]);
		card.SetCardIndex(_cardsCount);
        _activeCards.Add(cardModel, card);
        return card;
    }

    private void RemoveCard(BJCardModel cardModel)
    {
        BJCardController cardController = null;
        if (_activeCards.ContainsKey(cardModel))
        {
            cardController = _activeCards[cardModel];
            _activeCards.Remove(cardModel);
        }
        if (cardController != null)
        {
            _cardsPool.Add(cardController);
        }
    }

    private void UpdateScoreCounts()
    {
        _playerValueLabel.text = _gameModel.Player.PrimaryHand.Value.ToString();
        _dealerValueLabel.text = _gameModel.Dealer.PrimaryHand.Value.ToString();
    }

	IEnumerator MoveCardToDeck(BJCardController cardController)
	{
		Vector3 position = _cardPrefab.transform.position;
		iTween.MoveTo(cardController.gameObject, iTween.Hash("time", _cardMoveAnimationDuration, "position", position));
		yield return new WaitForSeconds(_cardMoveAnimationDuration);
	}

	private void DiscardPlayersHands()
	{
		if (_enableDebug)
		{
			Debug.Log("<color=blue> Discard Players Hands </color>");
		}


		foreach (BJCardModel card in _gameModel.Player.PrimaryHand.Cards)
		{
			_gameModel.DiscardsDekc.Add(card);
			card.IsFlipped = false;
		}
		_gameModel.Player.PrimaryHand.Cards = new List<BJCardModel>();

		foreach (BJCardModel card in _gameModel.Player.SecondaryHand.Cards)
		{
			_gameModel.DiscardsDekc.Add(card);
			card.IsFlipped = false;
		}
		_gameModel.Player.SecondaryHand.Cards = new List<BJCardModel>();

		foreach (BJCardModel card in _gameModel.Dealer.PrimaryHand.Cards)
		{
			_gameModel.DiscardsDekc.Add(card);
			card.IsFlipped = false;
		}
		_gameModel.Dealer.PrimaryHand.Cards = new List<BJCardModel>();

	}



    #endregion


    #region User Interactions

	void HandleOnMinusButtonAction ()
	{
		_currentBetIndex--;
		if (_currentBetIndex < 0)
		{
			_currentBetIndex = _betPerks.Length - 1;
		}
		
		_buttonsController.SetBetAmount(_betPerks[_currentBetIndex]);
		
		PlayerPrefsUtil.SetInt("_currentBetIndex", _currentBetIndex);

	}
	
	void HandleOnPlusButtonAction ()
	{
		_currentBetIndex++;
		if (_currentBetIndex > _betPerks.Length - 1)
		{
			_currentBetIndex = 0;
		}
		
		
		_buttonsController.SetBetAmount(_betPerks[_currentBetIndex]);
		
		PlayerPrefsUtil.SetInt("_currentBetIndex", _currentBetIndex);

	}

    void HandleOnDoubleButtonAction()
    {
		int betAmount = _betPerks[_currentBetIndex];
		if (PayCoins(betAmount))
		{
			_currentBetAmount += betAmount;
			StartCoroutine(PlayDouble());
			_buttonsController.SetBetAmount(_currentBetAmount);
		}

    }

    void HandleOnSplitButtonAction()
    {
		int betAmount = _betPerks[_currentBetIndex];
		if (PayCoins(betAmount))
		{
			//_currentBetAmount += betAmount;
			StartCoroutine(PlaySplit());
			_buttonsController.SetBetAmount(betAmount);
		}
    }

    void HandleOnStandButtonAction()
    {
		PlaySoundFX("Stand");
		StartCoroutine(PlayStand());
    }

    void HandleOnHitButtonAction()
    {
		_buttonsController.DisableDoubleButton();
		_buttonsController.DisableSplitButton();
        StartCoroutine(DealCardToPlayer());
    }

    void HandleOnDealButtonAction()
    {
 		int betAmount = _betPerks[_currentBetIndex];
		if (PayCoins(betAmount))
		{
			StartCoroutine(NewRound());
			_currentBetAmount = betAmount;
		}
    }

    #endregion

}

/*

#region GameplayBase Subclassing

protected override void Init()
{
    if (PlayerPrefsUtil.HasKey("_currentBetIndex"))
    {
        _currentBetIndex = PlayerPrefsUtil.GetInt("_currentBetIndex");
    }
    _betLabel.text = _betPerks[_currentBetIndex].ToString();


    _buttonsController.Init();
    _buttonsController.OnDealButtonAction += HandleOnDealButtonAction;
    _buttonsController.OnHitButtonAction += HandleOnHitButtonAction;
    _buttonsController.OnStandButtonAction += HandleOnStandButtonAction;
    _buttonsController.OnSplitButtonAction += HandleOnSplitButtonAction;
    _buttonsController.OnDoubleButtonAction += HandleOnDoubleButtonAction;
    _buttonsController.OnPlusButtonAction += HandleOnPlusButtonAction;
    _buttonsController.OnMinusButtonAction += HandleOnMinusButtonAction;
    _popupsController.Init();
}

protected override void GameStarted (string gameVariables)
{
    base.GameStarted (gameVariables);
    StartGame();
}



#endregion


#region Private - Gameplay

private void ShuffleDeck()
{
    if (_deckCards == null)
    {
        _deckCards = new List<BJCardController>();
        for (int i=0; i< _cardsSprites.Length ; i++)
        {
            BJCardController card = Instantiate(_cardPrefab) as BJCardController;
            _deckCards.Add(card);

            card.SetCard(i, _cardsSprites[i]);
        }
    }
}


private void StartGame()
{
    _currentCardsIndexes = 0;
    ShuffleDeck();

    _buttonsController.DispalyDeal();

}

IEnumerator StartRound()
{
    _playersHand = new List<BJCardController>();
    _delaerHand = new List<BJCardController>();

    _buttonsController.RoundStarted();

    _doubleActivated = false;

    yield return StartCoroutine(DealToDealer(false));
    yield return StartCoroutine(DealToDealer(true));
    yield return StartCoroutine(DealToPlayer());
    yield return StartCoroutine(DealToPlayer());
}

IEnumerator DealToPlayer()
{
    BJCardController card = GetNextCard();
    bool finished = false;
    _playersHand.Add(card);
    for (int i=0; i< _playersHand.Count ; i++)
    {
        BJCardController cardController = _playersHand[i];
        Vector3 position = _playerPile1Mark.transform.position;
        position.x  +=  0.7f * i - (0.7f * _playersHand.Count * 0.2f);
        System.Action oncompleteaction = ()=>
        {
            card.FlipCard(true, ()=>
                          {

            });
            finished = true;

        };
        iTween.MoveTo(cardController.gameObject, iTween.Hash("time",  0.3f, "x", position.x , "y" , position.y, "oncompleteaction", oncompleteaction ));
    }

    while (!finished)
    {
        yield return null;
    }
    UpdateScoreCounts();
    yield return null;

    if (_doubleActivated)
    {
        StartCoroutine(PlayDealer());
    }
    else
    {
        if (PlayerScore() >= 21)
        {
            yield return StartCoroutine(FinishRound());
        }
        else
        {
            if (_playersHand.Count == 2)
            {
                _buttonsController.SetDoubleState(true);

                if (_playersHand[0].Value == _playersHand[1].Value)
                {
                    _buttonsController.SetSplitState(true);
                }
            }
        }
    }
}

IEnumerator DealToDealer(bool isFlipped)
{
    BJCardController card = GetNextCard();
    bool finished = false;
    _delaerHand.Add(card);

    if (isFlipped)
    {
        card.FlipCard(false, null);
    }

    for (int i=0; i< _delaerHand.Count ; i++)
    {
        BJCardController cardController = _delaerHand[i];
        Vector3 position = _dealerPileMark.transform.position;
        position.x  += 0.7f * i - (0.7f * _delaerHand.Count * 0.2f);
        System.Action oncompleteaction = ()=>
        {
            if (isFlipped)
            {
                card.FlipCard(true, ()=>
                              {

                });
            }
            finished = true;

        };
        iTween.MoveTo(cardController.gameObject, iTween.Hash("time",  0.3f, "x", position.x , "y" , position.y, "oncompleteaction", oncompleteaction));

    }

    while (!finished)
    {
        yield return null;
    }

    UpdateScoreCounts();
    yield return null;
}


IEnumerator PlayDealer()
{
    _buttonsController.SetAllButtonsState(false);
    yield return StartCoroutine(FlipAllDelaerCards());

    while (DelaerScore() < 17)
    {
        yield return StartCoroutine(DealToDealer(true));
    }

    yield return null;

    StartCoroutine(FinishRound());
}

IEnumerator FlipAllDelaerCards()
{
    foreach (BJCardController card in _delaerHand)
    {
        if (!card.IsFlipped)
        {
            yield return StartCoroutine(FlipCard(card));
            UpdateScoreCounts();
        }
    }

}

IEnumerator FlipCard(BJCardController card)
{
    bool finished = false;
    if (!card.IsFlipped)
    {
        card.FlipCard(true, ()=>
                      {
            finished = true;
        });
    }
    else
    {
        finished = true;
    }

    while (!finished)
    {
        yield return null;
    }
}


private void UpdateScoreCounts()
{
    _playerValueLabel.text = PlayerScore().ToString();
    _dealerValueLabel.text = DelaerScore().ToString();
}


private int PlayerScore()
{
    return ScoreForHand(_playersHand);
}

private int DelaerScore()
{
    return ScoreForHand(_delaerHand);
}

private int ScoreForHand(List<BJCardController> cards)
{
    int highScore = 0;
    int lowScore = 0;
    foreach (BJCardController card in cards)
    {
        if (card.IsFlipped)
        {
            if (card.Value == 11)
            {
                highScore += 11;
                lowScore += 1;
            }
            else
            {
                highScore += card.Value;
                lowScore += card.Value;
            }
        }

    }

    if (highScore > 21)
    {
        return lowScore;
    }
    return highScore;
}

private BJCardController GetNextCard()
{
    if (_deckCards.Count == 0)
    {
        foreach (BJCardController usedCard in _usedCards)
        {
            if (!_playersHand.Contains(usedCard) && !_delaerHand.Contains(usedCard))
            {
                _deckCards.Add(usedCard);
            }
        }
    }
    BJCardController card = _deckCards[Random.Range(0, _deckCards.Count)];
    card.DealingCard(++_currentCardsIndexes);
    _deckCards.Remove(card);
    if (_usedCards == null)
    {
        _usedCards = new List<BJCardController>();
    }
    _usedCards.Add(card);
    return card;
}

IEnumerator FinishRound()
{
    yield return new WaitForSeconds(1.0f);

    int playerScore = PlayerScore();
    int dealerScore = DelaerScore();

    yield return StartCoroutine(FlipAllDelaerCards());
    UpdateScoreCounts();

    if ((playerScore > dealerScore && playerScore <= 21) || dealerScore > 21)
    {
        Debug.Log("player won!");

        AddCoins(_currentBetAmount * 2);
        _popupsController.DisplayYouWin(_currentBetAmount * 2);
        _currentBetAmount = 0;
    }
    else if ((dealerScore > playerScore && dealerScore <= 21) || playerScore > 21)
    {
        Debug.Log("dealer won!");

        _popupsController.DisplayYouLose();
    }
    else if (dealerScore == playerScore && playerScore <= 21 && dealerScore <= 21)
    {
        Debug.Log("tie!");
    }

    yield return new WaitForSeconds(1.0f);

    yield return StartCoroutine(ClearTable());

    //yield return StartCoroutine(StartRound());
    _buttonsController.DispalyDeal();
}

IEnumerator ClearTable()
{
    List<BJCardController> cardsInHands = new List<BJCardController>();
    cardsInHands.AddRange(_playersHand);
    cardsInHands.AddRange(_delaerHand);

    foreach (BJCardController card in cardsInHands)
    {
        iTween.MoveTo(card.gameObject, iTween.Hash("time", 0.3f, "x", -6.0f, "y", 6.0f));
        yield return new WaitForSeconds(0.1f);
    }

    _playersHand.Clear();
    _delaerHand.Clear();

    yield return null;
}


#endregion





#region User Interactions


void HandleOnMinusButtonAction ()
{
    _currentBetIndex--;
    if (_currentBetIndex < 0)
    {
        _currentBetIndex = _betPerks.Length - 1;
    }

    _buttonsController.SetBetAmount(_betPerks[_currentBetIndex]);

    PlayerPrefsUtil.SetInt("_currentBetIndex", _currentBetIndex);

    _buttonsController.SetDealButtonState(CanPayAmount(_betPerks[_currentBetIndex]));
}

void HandleOnPlusButtonAction ()
{
    _currentBetIndex++;
    if (_currentBetIndex > _betPerks.Length - 1)
    {
        _currentBetIndex = 0;
    }


    _buttonsController.SetBetAmount(_betPerks[_currentBetIndex]);

    PlayerPrefsUtil.SetInt("_currentBetIndex", _currentBetIndex);

    _buttonsController.SetDealButtonState(CanPayAmount(_betPerks[_currentBetIndex]));
}

void HandleOnDoubleButtonAction ()
{
    int betAmount = _betPerks[_currentBetIndex];
    if (CanPayAmount(betAmount))
    {
        if (PayCoins(_currentBetAmount))
        {
            _buttonsController.SetAllButtonsState(false);
            _currentBetAmount += betAmount;
            Debug.Log("pay: " + betAmount);
            _doubleActivated = true;
            StartCoroutine(DealToPlayer());
        }
    }
}

void HandleOnSplitButtonAction ()
{

}

void HandleOnStandButtonAction ()
{
    StartCoroutine(PlayDealer());
}

void HandleOnHitButtonAction ()
{
    StartCoroutine(DealToPlayer());
    _buttonsController.SetDoubleState(false);
    _buttonsController.SetSplitState(false);
}

void HandleOnDealButtonAction ()
{
    int betAmount = _betPerks[_currentBetIndex];
    if (CanPayAmount(betAmount))
    {
        if (PayCoins(_currentBetAmount))
        {
            StartCoroutine(StartRound());
            _currentBetAmount = betAmount;
            Debug.Log("pay: " + betAmount);
        }
    }
    _buttonsController.SetDealButtonState(false);

}

#endregion
}

*/
      