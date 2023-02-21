using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using UnityEngine;
using UnityEngine.Networking;
public class RestClient : MonoBehaviour
{
    //private const string URL = "https://api.lootlocker.io/server/leaderboards/Jungle1/list?count=10&after=0";
    private const string URL = "https://api.lootlocker.io/server/session";
    
    public IEnumerator SendPostRequest(Action<string> finishDelegate, WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post($"{URL}", form))
        {
            www.SetRequestHeader("LL-Version", "2021-03-01");
            www.SetRequestHeader("x-server-key", "dev_e492e96b25414f1aa0a15abc9a07ab3f");
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            finishDelegate(www.downloadHandler.text);
        }
    }
    public IEnumerator SendPostRequest(NetworkConnection conn, Action<string> finishDelegate, WWWForm form, string lastPartUrl)
    {
        using (UnityWebRequest www = UnityWebRequest.Post($"{URL}{lastPartUrl}", form))
        {
            www.SetRequestHeader("user-agent", "'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36'");
            yield return www.SendWebRequest();

            ReturnResponse(conn, finishDelegate, www);
        }
    }
    public IEnumerator SendPostRequest(NetworkConnection conn, List<Action<NetworkConnection, string>> finishDelegates, WWWForm form, string lastPartUrl)
    {
        using (UnityWebRequest www = UnityWebRequest.Post($"{URL}{lastPartUrl}", form))
        {
            yield return www.SendWebRequest();

            foreach (var finishDelegate in finishDelegates)
            {
                ReturnResponse(conn, finishDelegate, www);
            }
        }
    }

    public IEnumerator SendGetRequest(NetworkConnection conn, Action<NetworkConnection, string> finishDelegate, string lastPartUrl)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{URL}{lastPartUrl}"))
        {
            www.SetRequestHeader("LL-Version", "2021-03-01");
            www.SetRequestHeader("x-server-key", "dev_e492e96b25414f1aa0a15abc9a07ab3f");
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            ReturnResponse(conn, finishDelegate, www);
        }
    } 
    public IEnumerator SendGetRequest(Action<string> finishDelegate)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{URL}"))
        {
            www.SetRequestHeader("LL-Version", "2021-03-01");
            www.SetRequestHeader("x-server-key", "dev_e492e96b25414f1aa0a15abc9a07ab3f");
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            finishDelegate(www.downloadHandler.text);
        }
    }
    private void ReturnResponse(NetworkConnection conn, Action<NetworkConnection, string> finishDelegate, UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            //ErrorDisplayer.Instance.DisplayError("Error: " + www.error);
        }
        else
        {
            //DebugManager.Instance.Log("Returning response: " + www.downloadHandler.text, LogType.Response);
        }

        if (finishDelegate != null)
        {
            finishDelegate(conn, www.downloadHandler.text);
        }
    }
    private void ReturnResponse(NetworkConnection conn, Action<string> finishDelegate, UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            //ErrorDisplayer.Instance.DisplayError("Error: " + www.error);
        }
        else
        {
            //DebugManager.Instance.Log("Returning response: " + www.downloadHandler.text, LogType.Response);
        }

        if (finishDelegate != null)
        {
            finishDelegate(www.downloadHandler.text);
        }
    }
}
