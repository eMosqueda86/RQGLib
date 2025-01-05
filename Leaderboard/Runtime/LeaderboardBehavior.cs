using System;
using UnityEngine;
// using UnityEditor;

namespace RQGLib.Leaderboard
{
    
    
    
    [Serializable]
    public class LeaderboardBehavior : MonoBehaviour
    {
        public Leaderboard Leaderboard;
        
        public string Status = string.Empty;
        private int _leaderboardRange => Reference.Settings.LeaderboardRange;
        public DataContainer.PlayerData Player;
        public void StartSession() => StartCoroutine(Requests.StartSession(Player));

        public void SubmitScore() => StartCoroutine(Requests.SubmitScore(Player));
        
        public void SetPlayerName() => StartCoroutine(Requests.SetPlayerName(Player));
        
        private void GetPlayerRank() => StartCoroutine(Requests.GetPlayerRank(Player));
        
        public void GetResponseListReference() => Leaderboard.Ranks = Requests.ResponseList;

        public void GetTopPlayersLeaderboardList() => StartCoroutine(Requests.GetScoresList(_leaderboardRange));

        public void GetPlayerLeaderboardList()
        {
            if (Player.Rank > _leaderboardRange)
                StartCoroutine(Requests.GetScoresList(_leaderboardRange, Player.Rank - _leaderboardRange /2 ));
            else GetTopPlayersLeaderboardList();
        }
    }
}