using System;
using System.Collections.Generic;

namespace RQGLib.Leaderboard
{
    [Serializable]
    public static class DataContainer
    {
        
        //player data is created to track data coming from the leaderboard
        //all other data containers are used by methods in the requests class
        //all you need to do is create the player data and use the methods in leaderboard behavior
        
        [Serializable]
        public class PlayerData
        {
            public int Rank;
            public string Name;
            public string SessionToken;
            public string PlayerIdentifier;
            public string MemberID;
            public string Metadata = "";
            public Game GameData = new Game();
            public bool SeenBefore = false;
            public PlayerData(string name = null) {Name = name;}
            [Serializable]
            public class Game
            {
                public int Score = 0;
            }
        }
        
        [Serializable]
        public struct NewSessionRequest
        {
            public string session_token;
            public string player_id;
            public string player_identifier;
            public bool seen_before;
            public string metadata;
        }

        [Serializable]
        public struct SubmitScoreRequest
        {
            public string member_id;
            public int rank;
            public int score;
            public string metadata;
        }

        [Serializable]
        public struct ScoresListRequest
        {
            public Pagination pagination;
            public List<ResponseItem> items;
            public class Pagination
            {
                public int total;
                public int next_cursor;
                public int previous_cursor;
            }
        }
        
        [Serializable]
        public struct ResponseItem
        {
            public string member_id;
            public int rank;
            public int score;
            public Player player;
            public string metadata;
            [Serializable]
            public struct Player
            {
                public int id;
                public string public_uid;
                public string name;
                // public string ulid;
            }
        }
    }
}