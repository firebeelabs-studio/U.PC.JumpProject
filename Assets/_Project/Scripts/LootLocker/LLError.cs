using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LLError
{
    [JsonProperty("error")]
    public string Error;
    [JsonProperty("message")]
    public string Message;
}
