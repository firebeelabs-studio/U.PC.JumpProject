using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Skin")]
public class SwampieSkin : ScriptableObject
{
    public SkinType skinType;
    public SwampieType swampieType;
    public string SkinName;
    public Sprite SkinSprite;
    public List<SkinTransform> Positions;
    public Color color;
    
    [System.Serializable]
    public struct SkinTransform
    {
        public Vector3 Pos;
        public Quaternion Rot;
        public Vector3 Scale;
    }
    public enum SkinType
    {
        Hat,
        Jacket,
        Eyes,
        Mouth
    }
    public enum SwampieType
    {
        Turquoise,
        Yellow,
        Purple,
        Green,
        Blue
    }
}
