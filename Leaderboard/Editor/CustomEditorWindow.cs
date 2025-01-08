using UnityEditor;
using UnityEngine;

namespace RQGLib.Leaderboard
{
    public class CustomLeaderboardWindow : EditorWindow
    {
        
        [MenuItem("Window/Leaderboard Settings")]
        private static void ShowWindow() => GetWindow<CustomLeaderboardWindow>("Leaderboard Settings");
        [SerializeField] private LeaderboardSettings _settings;
        
        #if UNITY_EDITOR
        private void OnGUI()
        {
            _settings.GameKey = EditorGUILayout.TextField("Game Key",_settings?.GameKey);
            _settings.LeaderboardID = EditorGUILayout.TextField("Leaderboard ID", _settings?.LeaderboardID);
            _settings.Platform = EditorGUILayout.TextField("Platform", _settings?.Platform);
            _settings.LeaderboardRange = EditorGUILayout.IntField("Leaderboard Range", _settings?.LeaderboardRange ?? 0);
            AssetDatabase.SaveAssets();
        }
        
        #endif
    }
}