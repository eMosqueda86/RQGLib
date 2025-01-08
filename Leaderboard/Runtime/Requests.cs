using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace RQGLib.Leaderboard
{
    public class Requests
    {
        public static event Action StartSessionStartSuccessful;
        public static event Action SubmitScoreSuccessful;
        public static event Action GetPlayerRankSuccessful;
        public static event Action GetScoresListSuccessful;
        public static event Action SetPlayerNameSuccessful;
        public static event Action StartSessionStartFailed;
        public static event Action SubmitScoreFailed;
        public static event Action GetPlayerRankFailed;
        public static event Action GetScoresListFailed;
        public static event Action SetPlayerNameFailed;
        private static string _currentSession;
        public static List<DataContainer.ResponseItem> ResponseList;
        
        private static void TriggerAction(Action trigger)
        {
            trigger?.Invoke();
            ClearActionInvocationList(trigger);
        }
        
        private static void ClearActionInvocationList(Action trigger)
        {
            if (trigger?.GetInvocationList().Length > 0)
                foreach (Action action in trigger.GetInvocationList())
                    trigger -= action;
        }
        
        public static IEnumerator StartSession(DataContainer.PlayerData player)
        {
            string url = Reference.NewSessionURL;
            string data = Reference.GetNewSessionData(player);
            using (UnityWebRequest request = UnityWebRequest.Post(url, data,"application/json" ))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                { 
                    TriggerAction(StartSessionStartFailed);
                    ClearActionInvocationList(StartSessionStartSuccessful);
                }
                else
                {
                    string json = request.downloadHandler.text;
                    DataContainer.NewSessionRequest newSessionRequest = JsonUtility.FromJson<DataContainer.NewSessionRequest>(json);
                    player.PlayerIdentifier = newSessionRequest.player_identifier;
                    player.MemberID = newSessionRequest.player_id;
                    player.SessionToken = newSessionRequest.session_token;
                    player.Metadata = newSessionRequest.metadata;
                    player.SeenBefore = newSessionRequest.seen_before;
                    _currentSession = newSessionRequest.session_token;
                    TriggerAction(StartSessionStartSuccessful);
                }
            }
        }
        public static IEnumerator SubmitScore(DataContainer.PlayerData player)
        {
            string url = Reference.SubmitScoreURL;
            string submitData = Reference.GetSubmitScoreData(player);
            using (UnityWebRequest request = UnityWebRequest.Post(url, submitData, "application/json"))
            {
                request.SetRequestHeader("x-session-token", player.SessionToken);
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    TriggerAction(SubmitScoreFailed);
                    ClearActionInvocationList(SubmitScoreSuccessful);
                }
                else
                {
                    string json = request.downloadHandler.text;
                    DataContainer.SubmitScoreRequest submitScoreRequest = JsonUtility.FromJson<DataContainer.SubmitScoreRequest>(json);
                    player.MemberID = submitScoreRequest.member_id;
                    TriggerAction(SubmitScoreSuccessful);
                }
            }
        }

        public static IEnumerator GetPlayerRank(DataContainer.PlayerData player)
        {
            string url = Reference.GetPlayerRankURL(player);
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("x-session-token", player.SessionToken);
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    TriggerAction(GetPlayerRankFailed);
                    ClearActionInvocationList(GetPlayerRankFailed);
                }
                else
                {
                    string json = request.downloadHandler.text;
                    DataContainer.ResponseItem responseItem = JsonUtility.FromJson<DataContainer.ResponseItem>(json);
                    player.Rank = responseItem.rank;
                    TriggerAction(GetPlayerRankSuccessful);
                }
            }
        }

        public static IEnumerator GetScoresList(int count, int after = 0)
        {
            string url = Reference.GetScoresListURL(count, after);
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("x-session-token", _currentSession);
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    TriggerAction(GetScoresListFailed);
                    ClearActionInvocationList(GetScoresListSuccessful);
                }
                else
                {
                    string json = request.downloadHandler.text;
                    DataContainer.ScoresListRequest scoresList = JsonUtility.FromJson<DataContainer.ScoresListRequest>(json);
                    ResponseList = scoresList.items;
                    TriggerAction(GetScoresListSuccessful);
                }
            }
        }
        
        public static IEnumerator SetPlayerName(DataContainer.PlayerData player)
        {
            UnityWebRequest request = GetPatchRequest(Reference.SetNameURL, Reference.GetPlayerNameData(player));
            request.SetRequestHeader("x-session-token", player.SessionToken);
            request.SetRequestHeader("LL-Version", "2021-03-01");
            yield return request.SendWebRequest();
            if(request.result != UnityWebRequest.Result.Success)
            {
                TriggerAction(SetPlayerNameFailed);
                ClearActionInvocationList(SetPlayerNameSuccessful);
            }
            else
            {
                string json = request.downloadHandler.text;
                DataContainer.ResponseItem responseItem = JsonUtility.FromJson<DataContainer.ResponseItem>(json);
                player.Name = responseItem.player.name;
                TriggerAction(SetPlayerNameSuccessful);
            }
        }

        private static UnityWebRequest GetPatchRequest(string url, string postData) => 
            SetupPatch(new UnityWebRequest(url, "PATCH"), postData, "application/json");
        private static UnityWebRequest SetupPatch(UnityWebRequest request, string postData, string contentType)
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            if (string.IsNullOrEmpty(postData)) request.SetRequestHeader("Content-Type", contentType);
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.uploadHandler = new UploadHandlerRaw(bytes);
                request.uploadHandler.contentType = contentType;
            }
            return request;
        }
    }
}