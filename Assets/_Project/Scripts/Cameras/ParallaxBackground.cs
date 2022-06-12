using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float _length;
    private float _startpos;
    [SerializeField] float parallaxEffect;
    [SerializeField] GameObject Cam;

    void Start()
    {
        _startpos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float _temp = Cam.transform.position.x * (1 - parallaxEffect);
        float _dist = (Cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(_startpos + _dist, transform.position.y, transform.position.z);

        if (_temp > _startpos + _length) _startpos += _length;
        else if (_temp < _startpos - _length) _startpos -= _length;
    }
}
