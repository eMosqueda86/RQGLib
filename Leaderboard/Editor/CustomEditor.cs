using System;
using UnityEditor;
using UnityEngine;
// using UnityEngine.UI;

namespace RQGLib.Leaderboard
{
    [UnityEditor.CustomEditor(typeof(LeaderboardBehavior))]
    public class CustomEditor : Editor
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
    
    public class CustomLeaderboardWindow : EditorWindow
    {
        [MenuItem("Window/Leaderboard")]
        private static void ShowWindow() => GetWindow<CustomLeaderboardWindow>("Leaderboard");
        [SerializeField] private LeaderboardSettings _settings;
        private void OnGUI()
        {
            _settings = Reference.Settings;
            Reference.Settings.GameKey = EditorGUILayout.TextField("Game Key",_settings?.GameKey ?? Reference.Settings.GameKey);
            Reference.Settings.LeaderboardID = EditorGUILayout.TextField("Leaderboard ID", _settings?.LeaderboardID ?? Reference.Settings.LeaderboardID);
            Reference.Settings.LeaderboardRange = EditorGUILayout.IntField("Leaderboard Range", _settings?.LeaderboardRange ?? Reference.Settings.LeaderboardRange);
            AssetDatabase.SaveAssets();
        }
    }
}
