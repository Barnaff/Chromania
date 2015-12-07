using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void LobbyUpdatedDelegate(LobbyDataModel lobbyData);

public interface IPortal  {

	string PortalIdentifier { get; }

	bool IsTurnementEnabled { get; }

	string TermsOfUseURL { get; }

	PortalType GetPortalType { get; }

	void LoadPortalConfiguration(System.Action <LobbyDataModel> completionAction);

	List<TileDataModelAbstract> LobbyTilesList { get; }

	event LobbyUpdatedDelegate OnLobbyUpdated;

	TileDataModelAbstract GetTileForKey(string key);
}
