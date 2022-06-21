using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private GameObject _cam;
    [System.Serializable]
    public class ParallaxBackgroundParts
    {
        public GameObject BackgroundPart;
        public float ParallaxEffectPower;
        [HideInInspector] public float Length;
        [HideInInspector] public float StartPos;
    }
    [SerializeField] private List<ParallaxBackgroundParts> backgroundParts;

    void Awake()
    {
        foreach (ParallaxBackgroundParts part in backgroundParts)
        {
            part.Length = part.BackgroundPart.GetComponent<SpriteRenderer>().bounds.size.x;
            part.StartPos = part.BackgroundPart.transform.position.x;
        }
    }
    private void LateUpdate()
    {
        foreach (ParallaxBackgroundParts part in backgroundParts)
        {
            float _temp = _cam.transform.position.x * (1 - part.ParallaxEffectPower);
            float _dist = _cam.transform.position.x * part.ParallaxEffectPower - 3f;
            part.BackgroundPart.transform.position = new Vector3(part.StartPos + _dist, part.BackgroundPart.transform.position.y, part.BackgroundPart.transform.position.z);
            if (_temp > part.StartPos + part.Length/2)
            {
                part.StartPos += part.Length;
            }
            else if (_temp < part.StartPos - part.Length)
            {
                part.StartPos -= part.Length;
            }
        }
    }
}