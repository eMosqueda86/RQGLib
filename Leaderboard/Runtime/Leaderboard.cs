using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQGLib.Leaderboard
{
    [CreateAssetMenu(fileName = "Leaderboard", menuName = "Scriptable Objects/Leaderboard")]
    [Serializable]
    public class Leaderboard : ScriptableObject
    {
        public List<DataContainer.ResponseItem> Ranks;
    }
}