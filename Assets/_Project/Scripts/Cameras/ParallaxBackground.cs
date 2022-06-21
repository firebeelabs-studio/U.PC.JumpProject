using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private GameObject _cam;
    [System.Serializable]
    public class ParallaxBackgroundPart
    {
        public GameObject BackgroundPart;
        public float ParallaxEffectPower;
        public float length { get; set; }
    }
    [SerializeField] private List<ParallaxBackgroundPart> backgroundParts;
    private float _startpos;

    void Awake()
    {
        foreach (ParallaxBackgroundPart backgroundPart in backgroundParts)
        {
            backgroundPart.length = backgroundPart.BackgroundPart.GetComponent<SpriteRenderer>().bounds.size.x;
        }
    }
    void Start()
    {
        _startpos = transform.position.x;
        foreach (ParallaxBackgroundPart item in backgroundParts)
        {
            print(item.length);
        }
    }
    private void FixedUpdate()
    {
        foreach (ParallaxBackgroundPart backgroundPart in backgroundParts)
        {
            float _temp = _cam.transform.position.x * (1 - backgroundPart.ParallaxEffectPower);
            float _dist = (_cam.transform.position.x * backgroundPart.ParallaxEffectPower);
            if (_temp > _startpos + backgroundPart.length)
            {
                _startpos += backgroundPart.length;
            }
            else if (_temp < _startpos - backgroundPart.length)
            {
                _startpos -= backgroundPart.length;
            }
            transform.position = new Vector3(_startpos + _dist, transform.position.y, transform.position.z);
        }
    }
}