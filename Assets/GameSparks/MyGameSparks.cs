using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
	public class LogEventRequest_POST_CS_SCORE : GSTypedRequest<LogEventRequest_POST_CS_SCORE, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_POST_CS_SCORE() : base("LogEventRequest"){
			request.AddString("eventKey", "POST_CS_SCORE");
		}
		public LogEventRequest_POST_CS_SCORE Set_SCORE( long value )
		{
			request.AddNumber("SCORE", value);
			return this;
		}			
		public LogEventRequest_POST_CS_SCORE Set_COLOR_1( long value )
		{
			request.AddNumber("COLOR_1", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_POST_CS_SCORE : GSTypedRequest<LogChallengeEventRequest_POST_CS_SCORE, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_POST_CS_SCORE() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "POST_CS_SCORE");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_POST_CS_SCORE SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_POST_CS_SCORE Set_SCORE( long value )
		{
			request.AddNumber("SCORE", value);
			return this;
		}			
		public LogChallengeEventRequest_POST_CS_SCORE Set_COLOR_1( long value )
		{
			request.AddNumber("COLOR_1", value);
			return this;
		}			
	}
	
}
	
	
	
namespace GameSparks.Api.Requests{
	
	public class LeaderboardDataRequest_CS : GSTypedRequest<LeaderboardDataRequest_CS,LeaderboardDataResponse_CS>
	{
		public LeaderboardDataRequest_CS() : base("LeaderboardDataRequest"){
			request.AddString("leaderboardShortCode", "CS");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LeaderboardDataResponse_CS (response);
		}		
		
		/// <summary>
		/// The challenge instance to get the leaderboard data for
		/// </summary>
		public LeaderboardDataRequest_CS SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public LeaderboardDataRequest_CS SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_CS SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public LeaderboardDataRequest_CS SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public LeaderboardDataRequest_CS SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// The offset into the set of leaderboards returned
		/// </summary>
		public LeaderboardDataRequest_CS SetOffset( long offset )
		{
			request.AddNumber("offset", offset);
			return this;
		}
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_CS SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public LeaderboardDataRequest_CS SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public LeaderboardDataRequest_CS SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
		
	}

	public class AroundMeLeaderboardRequest_CS : GSTypedRequest<AroundMeLeaderboardRequest_CS,AroundMeLeaderboardResponse_CS>
	{
		public AroundMeLeaderboardRequest_CS() : base("AroundMeLeaderboardRequest"){
			request.AddString("leaderboardShortCode", "CS");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new AroundMeLeaderboardResponse_CS (response);
		}		
		
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public AroundMeLeaderboardRequest_CS SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_CS SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public AroundMeLeaderboardRequest_CS SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public AroundMeLeaderboardRequest_CS SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_CS SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_CS SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_CS SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
	}
}

namespace GameSparks.Api.Responses{
	
	public class _LeaderboardEntry_CS : LeaderboardDataResponse._LeaderboardData{
		public _LeaderboardEntry_CS(GSData data) : base(data){}
		public long? SCORE{
			get{return response.GetNumber("SCORE");}
		}
		public long? COLOR_1{
			get{return response.GetNumber("COLOR_1");}
		}
	}
	
	public class LeaderboardDataResponse_CS : LeaderboardDataResponse
	{
		public LeaderboardDataResponse_CS(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_CS> Data_CS{
			get{return new GSEnumerable<_LeaderboardEntry_CS>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_CS(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_CS> First_CS{
			get{return new GSEnumerable<_LeaderboardEntry_CS>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_CS(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_CS> Last_CS{
			get{return new GSEnumerable<_LeaderboardEntry_CS>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_CS(data);});}
		}
	}
	
	public class AroundMeLeaderboardResponse_CS : AroundMeLeaderboardResponse
	{
		public AroundMeLeaderboardResponse_CS(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_CS> Data_CS{
			get{return new GSEnumerable<_LeaderboardEntry_CS>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_CS(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_CS> First_CS{
			get{return new GSEnumerable<_LeaderboardEntry_CS>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_CS(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_CS> Last_CS{
			get{return new GSEnumerable<_LeaderboardEntry_CS>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_CS(data);});}
		}
	}
}	

namespace GameSparks.Api.Messages {


}
