using UnityEngine;
[System.Serializable]                                   //allows us to see list of objects of this class in inspector
public class ParallaxBackgroundParts
{
    public GameObject BackgroundPart;
    public float ParallaxEffectPower;
    [HideInInspector] public float Length;
    [HideInInspector] public float StartPosX;
    [HideInInspector] public float StartPosY;
}