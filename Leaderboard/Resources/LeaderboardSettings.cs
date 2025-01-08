using System;
using UnityEditor;
using UnityEngine;

namespace RQGLib.Leaderboard
{
    [CreateAssetMenu(fileName = "LeaderboardSettings", menuName = "Scriptable Objects/Leaderboard Settings")]
    [Serializable]
    public class LeaderboardSettings : ScriptableObject
    {
        public string GameKey;
        public string LeaderboardID;
        public int LeaderboardRange = 10;
        public string Platform;
        public static string AssetPath;
    
        private void OnEnable()
        {
            #if UNITY_EDITOR
            AssetPath = AssetDatabase.GetAssetPath(this);
            #endif
        }
    }
}