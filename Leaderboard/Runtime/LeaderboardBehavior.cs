using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQGLib.Leaderboard
{
    
    [Serializable]
    public class LeaderboardBehavior : MonoBehaviour
    {
        
        /*
         * these methods run coroutines which fetch data from the leaderboard backend
         * use Request.[StartSessionStartSuccessful,SubmitScoreSuccessful,GetPlayerRankSuccessful,
         * GetScoresListSuccessful,SetPlayerNameSuccessful,StartSessionStartFailed,SubmitScoreFailed;
         * GetPlayerRankFailed,GetScoresListFailed,SetPlayerNameFailed] for actions to complete on
         * completion or fail;
         */
        [SerializeField] public Leaderboard Leaderboard;
        [SerializeField] private List<DataContainer.ResponseItem> _ranks;
        public string Status = string.Empty;
        private int _leaderboardRange => Reference.Settings.LeaderboardRange;
        public DataContainer.PlayerData Player;
        
        // public void CreateNewPlayer() => Player = new DataContainer.PlayerData();
        //
        // public void SetPlayer(DataContainer.PlayerData player) => Player = player; 
        public void StartSession() => StartCoroutine(Requests.StartSession(Player));

        public void SubmitScore() => StartCoroutine(Requests.SubmitScore(Player));
        
        public void SetPlayerName() => StartCoroutine(Requests.SetPlayerName(Player));
        
        private void GetPlayerRank() => StartCoroutine(Requests.GetPlayerRank(Player));

        public void GetResponseListReference()
        {
            Leaderboard.Ranks = Requests.ResponseList;
            _ranks = Leaderboard.Ranks;
        } 

        public void GetTopPlayersLeaderboardList() => StartCoroutine(Requests.GetScoresList(_leaderboardRange));

        public void GetPlayerLeaderboardList()
        {
            if (Player.Rank > _leaderboardRange)
                StartCoroutine(Requests.GetScoresList(_leaderboardRange, Player.Rank - _leaderboardRange /2 ));
            else GetTopPlayersLeaderboardList();
        }
    }
}