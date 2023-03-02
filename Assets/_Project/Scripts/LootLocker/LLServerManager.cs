using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[RequireComponent(typeof(RestClient))]
public class LLServerManager : MonoBehaviour
{
    public string Token;
    private RestClient _restClient;

    private void Awake()
    {
        _restClient = GetComponent<RestClient>();
    }

    private IEnumerator Start()
    {
        for (;;)
        {
            string lastPartUrl = "server/session";
            byte[] s = System.Text.Encoding.UTF8.GetBytes("{ \"game_version\": \"1.0.0.0\" }"); 
            StartCoroutine(_restClient.SendPostRequest(GetToken, lastPartUrl,s));
            yield return new WaitForSecondsRealtime(1740);
        }
    }

    private void GetToken(string json)
    {
        Token = JsonConvert.DeserializeObject<LLToken>(json)?.Token;
    }
}
