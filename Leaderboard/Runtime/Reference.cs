using RQGLib.Leaderboard;
using UnityEditor;
using UnityEngine;

namespace RQGLib.Leaderboard
{
    public class Reference
    {
        //methods and properties that return endpoint urls, and data strings for fetch requests
        //more endpoints can be found in the game api section at lootlocker.com
        public static string LeaderboardSettingsPath => LeaderboardSettings.AssetPath;
        public static LeaderboardSettings Settings => AssetDatabase.LoadAssetAtPath<LeaderboardSettings>(LeaderboardSettingsPath);
        private static string _gameKey => Settings.GameKey;
        private static string _leaderboardID => Settings.LeaderboardID;
        private static string _platform => Settings.Platform;
        public static string NewSessionURL => GetSessionURL();
        public static string SetNameURL => "https://api.lootlocker.io/game/player/name";
        public static string SubmitScoreURL => $"https://api.lootlocker.io/game/leaderboards/{_leaderboardID}/submit";
        public static string GetNewSessionData(DataContainer.PlayerData player)
        {
            string sessionData = $"\"game_key\":\"{_gameKey}\"";
            if (!string.IsNullOrEmpty(_platform)) sessionData += $",\"platform\":\"{_platform}\"";
            if (!string.IsNullOrEmpty(player.PlayerIdentifier)) sessionData += $",\"player_identifier\":\"{player.PlayerIdentifier}\"";
            sessionData += ",\"game_version\":\"1.0.0\"";
            return "{" + sessionData + "}";
        }

        public static string GetSubmitScoreData(DataContainer.PlayerData player) => 
            "{" + $"\"score\":\"{player.GameData.Score}\",\"metadata\":\"{player.Metadata}\"" + "}";
        
        private static string GetSessionURL()
        {
            string sessionURL = $"https://api.lootlocker.io/game/v2/session";
            if (string.IsNullOrEmpty(_platform)) sessionURL += "/guest";
            return sessionURL;
        }

        public static string GetScoresListURL(int count, int after = 0)
        {
            string baseURL = $"https://api.lootlocker.io/game/leaderboards/{_leaderboardID}/list?count={count}";
            if(after > 0) baseURL += $"&after={after}";
            return baseURL;
        }

        public static string GetPlayerRankURL(DataContainer.PlayerData player) =>
            $"https://api.lootlocker.io/game/leaderboards/{_leaderboardID}/member/{player.MemberID}";

        public static string GetPlayerNameData(DataContainer.PlayerData player)
        {
            return $"{{\"name\":\"{player.Name}\"}}";
        }
        
    }
}