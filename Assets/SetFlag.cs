using UnityEngine;

public class SetFlag : MonoBehaviour
{
    [SerializeField] Transform _flag;
    [SerializeField] Transform _top;
    [SerializeField] Transform _mid;
    [SerializeField] float _speed;
    [SerializeField] float _height;

    Vector2 _endTopPos, _endMidPos;
    Vector3 _startScale, _endScale;
    bool _didItOnce;
    bool _activateCheckpoint;
    float _includeScale;

    private void Start()
    {
        _endTopPos = (Vector2)_top.position + new Vector2(0, _height);
        _endMidPos = (Vector2)_mid.position + new Vector2(0, _height/2);

        // this includes the scale of main object, 
        _includeScale = 1/_flag.transform.localScale.y;
        _startScale = _mid.transform.localScale;

        //height of scale is increased a bit, to prevent open space between two sprites
        _endScale = _startScale + new Vector3(0, _includeScale * _height, 0);
    }

    private void Update()
    {
        if (!_activateCheckpoint) return;
        SetFlagPos();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_didItOnce) return;
        _activateCheckpoint = true;
        _didItOnce = true;
    }

    void SetFlagPos()
    {
        // set the position of top & mid after collision with player
        // bcs the scale of mid is chaning, the endPos is half of the top distance, also the speed is 2x slower
        _top.position = Vector2.MoveTowards(_top.position, _endTopPos, _speed * Time.deltaTime);
        _mid.position = Vector2.MoveTowards(_mid.position, _endMidPos, (_speed/2) * Time.deltaTime);

        _mid.transform.localScale = Vector3.MoveTowards(_mid.transform.localScale, _endScale, _includeScale * _speed * Time.deltaTime);
    }
}
