using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float _length;
    private float _startpos;
    [SerializeField] private float _parallaxEffect;
    [SerializeField] private GameObject _cam;

    void Awake()
    {
        _startpos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float _temp = _cam.transform.position.x * (1 - _parallaxEffect);
        float _dist = (_cam.transform.position.x * _parallaxEffect);
        transform.position = new Vector3(_startpos + _dist, transform.position.y, transform.position.z);

        if (_temp > _startpos + _length)
        {
            _startpos += _length;
        }
        else if (_temp < _startpos - _length)
        {
            _startpos -= _length;
        }
    }
}
