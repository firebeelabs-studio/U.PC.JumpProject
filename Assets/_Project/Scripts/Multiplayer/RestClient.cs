using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using UnityEngine;
using UnityEngine.Networking;
public class RestClient : MonoBehaviour
{
    //private const string URL = "https://api.lootlocker.io/server/leaderboards/Jungle1/list?count=10&after=0";
    private const string URL = "https://cqk1q9qr.api.lootlocker.io/";
    
    public IEnumerator SendPostRequest(Action<string> finishDelegate,string lastPartUrl, byte[] body, string token = "")
    {
        using (UnityWebRequest www = UnityWebRequest.Put($"{URL}{lastPartUrl}", body))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("LL-Version", "2021-03-01");
            if (string.IsNullOrEmpty(token))
            {
                www.SetRequestHeader("x-server-key", "dev_e492e96b25414f1aa0a15abc9a07ab3f");
            }
            else
            {
                www.SetRequestHeader("x-auth-token", token);
            }
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            finishDelegate(www.downloadHandler.text);
        }
    }
    public IEnumerator SendGetRequest(Action<string> finishDelegate,string lastPartUrl, string token)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{URL}{lastPartUrl}"))
        {
            www.SetRequestHeader("x-auth-token", token);
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
