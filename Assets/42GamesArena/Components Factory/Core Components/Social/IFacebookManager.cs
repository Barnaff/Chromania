using System.Collections.Generic;

public interface IFacebookManager {

	bool FacebookEnabled { get; }

	void InitFacebookWithCompletion(System.Action completionBlock);

	void PerformFacebookLogin(System.Action <bool> loginCompletionBlock);

	void GetMyFacebookFriends(System.Action <List<IFacebookProfile>> completionBlock);

	void GetFacebookUserDetails(System.Action <IFacebookProfile> completionBlock, System.Action <ErrorObj> failBlock);

	void InviteFriend(IFacebookProfile friendProfile, System.Action <bool> sucsessBlock, System.Action <ErrorObj> failBlock);

	bool IsLoggedIn();

	void GetProfileForId(string facebookProfileId, System.Action <IFacebookProfile> completionAction);

	void ShareOnWall(string title, string caption, string url, System.Action <bool> completionAction);
}

