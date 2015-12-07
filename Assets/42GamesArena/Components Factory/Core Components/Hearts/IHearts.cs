using UnityEngine;
using System.Collections;

public delegate void HeartAddedDelegate();
public delegate void HeartRemovedDelegate();
public delegate void OutOfHeartsDelegate();

public interface IHearts  {

	bool HeartsEnabled { set ; get; }

	int HeartsCount { get; }

	int MaxHeartsCount { get; }

	System.TimeSpan TimeToNextHeart { get; }

	void ReduceHeart();

	void RefillHearts(int amount = 1);

	event HeartAddedDelegate OnHeartAdded;

	event HeartRemovedDelegate OnHeartRemoved;
	
	event OutOfHeartsDelegate OnOutOfHearts;
}
