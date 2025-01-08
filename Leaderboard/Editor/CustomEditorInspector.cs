using System;
using UnityEditor;
using UnityEngine;
// using UnityEngine.UI;

namespace RQGLib.Leaderboard
{
    [UnityEditor.CustomEditor(typeof(LeaderboardBehavior))]
    public class CustomEditorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            LeaderboardBehavior leaderboardBehavior = (LeaderboardBehavior)target;
            base.OnInspectorGUI();
            if (GUILayout.Button("New Player")) leaderboardBehavior.Player = new DataContainer.PlayerData();
            if (GUILayout.Button("Start Session")) leaderboardBehavior.StartSession();
            if (GUILayout.Button("Submit Score")) leaderboardBehavior.SubmitScore();
            if (GUILayout.Button("Set Player Name")) leaderboardBehavior.SetPlayerName();
            if (GUILayout.Button("Top Players"))
            {
                Requests.GetScoresListSuccessful += leaderboardBehavior.GetResponseListReference;
                leaderboardBehavior.GetTopPlayersLeaderboardList();
            }
            if (GUILayout.Button("Player Leaderboard"))
            {
                Requests.GetScoresListSuccessful += leaderboardBehavior.GetResponseListReference;
                leaderboardBehavior.GetPlayerLeaderboardList();
            }
        }
    }
    

}
