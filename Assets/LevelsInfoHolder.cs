using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelsInfoHolder : MonoBehaviour
{
    //initialize map from this one
    [SerializeField] public List<LevelInfo> LevelsInfo;
}

[Serializable]
public struct LevelInfo
{
    public string LevelName;
    public string SceneName;
    public bool IsAvailable;
    public Sprite LevelImage;
}
