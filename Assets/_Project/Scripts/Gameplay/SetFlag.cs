using UnityEngine;

public class SetFlag : MonoBehaviour, ICheckpointAnim
{
    [SerializeField] private Transform _flag;
    [SerializeField] private Transform _top;
    [SerializeField] private Transform _mid;
    [SerializeField] float _speed;
    [SerializeField] float _height;

    private Vector2 _endTopPos, _endMidPos;
    private Vector3 _startScale, _endScale;
    private bool _isCheckpointActivated;
    private float _includeScale;

    private void Start()
    {
        _endTopPos = (Vector2)_top.position + new Vector2(0, _height);
        _endMidPos = (Vector2)_mid.position + new Vector2(0, _height/2);

        // this includes the scale of main object and prevent open space
        _includeScale = 1 / _flag.transform.localScale.y;

        _startScale = _mid.transform.localScale;

        //height of scale is increased a bit, to prevent open space between two sprites
        _endScale = _startScale + new Vector3(0, _includeScale * _height, 0);
    }

    private void Update()
    {
        if (!_isCheckpointActivated) return; 

        if ((Vector2)_top.position == _endTopPos && ((Vector2)_mid.position == _endMidPos)) return; // it stops SetFlagPos after flag reaches top - better efficency
        SetFlagPos();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isCheckpointActivated) return;

        _isCheckpointActivated = true;
    }

    void SetFlagPos()
    {
        // set the position of top & mid after collision with player
        // bcs the scale of mid is chaning, the endPos is half of the top distance, also the speed is 2x slower
        _top.position = Vector2.MoveTowards(_top.position, _endTopPos, _speed * Time.deltaTime);
        _mid.position = Vector2.MoveTowards(_mid.position, _endMidPos, (_speed/2) * Time.deltaTime);

        _mid.transform.localScale = Vector3.MoveTowards(_mid.transform.localScale, _endScale, _includeScale * _speed * Time.deltaTime);
    }

    public void ResetToDefaultState()
    {
        _isCheckpointActivated = false;
    }
}
