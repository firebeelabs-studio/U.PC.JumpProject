using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class Pagination : MonoBehaviour
{
    [JsonProperty("total")]
    public int Total;
    [JsonProperty("next_cursor")]
    public int NextCursor;
    [JsonProperty("previous_cursor")]
    public int? PreviousCursor;
}
