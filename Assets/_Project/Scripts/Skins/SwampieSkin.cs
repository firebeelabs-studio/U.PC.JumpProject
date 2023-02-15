using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "New Skin", menuName = "Skin")]
public class SwampieSkin : ScriptableObject
{
    public string Id;
    public SkinType skinType;
    public SwampieType swampieType;
    public string SkinName;
    public Sprite SkinSprite;
    public Sprite DisplaySprite;
    public List<SkinTransform> Positions;
    public Color color;
    
    [System.Serializable]
    public struct SkinTransform
    {
        public Vector3 Pos;
        public Quaternion Rot;
        public Vector3 Scale;
    }
    [Serializable]
    public enum SkinType
    {
        Hat,
        Jacket,
        Eyes,
        Mouth,
        Body
    }
    [Serializable]
    public enum SwampieType
    {
        Turquoise,
        Yellow,
        Purple,
        Green,
        Blue
    }
    
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            AssignNewUID();
        }
    }

    private void Reset()
    {
        AssignNewUID();
    }

    public void AssignNewUID()
    {
        Id = System.Guid.NewGuid().ToString();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
