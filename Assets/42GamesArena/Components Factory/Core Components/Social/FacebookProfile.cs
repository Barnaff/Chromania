using UnityEngine;
using System.Collections.Generic;
using Facebook.MiniJSON;

public class FacebookProfile : Object, IFacebookProfile 
{
	#region IFacebookProfile implementation

	public string UserName { get; set; }
	public string FirstName  { get; set; }
	public string LastName  { get; set; }
	public string UserToken  { get; set; }
	public string FacebookId  { get; set; }
	public string UserImageURL { get; set; }
	
	#endregion

	public static FacebookProfile CreateProfile(string userName, string firstName, string lastName, string userToken, string facebookId, string userImageURL)
	{
		FacebookProfile facebookProfile = new FacebookProfile();
		facebookProfile.UserName = userName;
		facebookProfile.FirstName = firstName;
		facebookProfile.LastName = lastName;
		facebookProfile.UserToken = userToken;
		facebookProfile.FacebookId = facebookId;
		facebookProfile.UserImageURL = userImageURL;
		return facebookProfile;
	}
	
	public static FacebookProfile CreateWithResult(FBResult result)
	{
		FacebookProfile facebookProfile = new FacebookProfile();
		var dict = Json.Deserialize(result.Text) as Dictionary<string,object>;
		facebookProfile.UserName = dict["name"].ToString();
		facebookProfile.FirstName = dict["first_name"].ToString();
		facebookProfile.LastName = dict["last_name"].ToString();
		facebookProfile.FacebookId = dict["id"].ToString();
		facebookProfile.UserImageURL = "https://graph.facebook.com/" + facebookProfile.FacebookId.ToString() + "/picture?type=large&width=200&height=200";
		return facebookProfile;;
	}

	public override string ToString ()
	{
		return string.Format ("<color=cyan>[FacebookProfile: UserName={0}, FirstName={1}, LastName={2}, UserToken={3}, FacebookId={4}, UserImageURL={5}]</color>", UserName, FirstName, LastName, UserToken, FacebookId, UserImageURL);
	}
}
