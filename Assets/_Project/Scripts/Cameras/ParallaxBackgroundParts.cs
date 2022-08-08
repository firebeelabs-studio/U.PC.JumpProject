using UnityEngine;
[System.Serializable]
public class ParallaxBackgroundParts
{
    public GameObject BackgroundPart;
    public float ParallaxEffectPower;
    [HideInInspector] public float Length;
    [HideInInspector] public float StartPosX;
    [HideInInspector] public float StartPosY;
}