using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    [Tooltip("If true player wouldn't be able to move until grounded")]
    [SerializeField] private bool _blockMovement = false;
    [SerializeField] private float _bounceForce = 20;
    [SerializeField] private float _horizontalBoost = 1;
    [SerializeField] private float _cameraZoomOutDuration;

    //[SerializeField] private JumpSimulation _jumpSimulation;

    //sounds
    [SerializeField] private AudioClip _bouncerSound;

    private AudioPlayer _audioPlayer;
    private Vector2 _bounceDirectionVector;
    private bool _cancelMovement = true;
    private float sinDegree;
    private float cosDegree;

    public float Vx => sinDegree * _bounceForce * _horizontalBoost;
    public float Vy => cosDegree * _bounceForce;

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Start()
    {
        CalculateForces();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (collision.TryGetComponent(out IPawnController controller))
        {
            _audioPlayer.PlayOneShotSound(_bouncerSound);
            controller.AddForce(_bounceDirectionVector * _bounceForce, PlayerForce.Burst,true, _blockMovement);
        }
    }

    private void CalculateForces()
    {
        float degreeInRadians = (transform.transform.eulerAngles.z * (Mathf.PI)) / 180;
        sinDegree = Mathf.Sin(degreeInRadians);
        cosDegree = Mathf.Cos(degreeInRadians);
        _bounceDirectionVector = new Vector2(-sinDegree * _horizontalBoost, cosDegree);
    }

    [ContextMenu("Create Path")]
    private void SetPath()
    {
        CalculateForces();
        //_jumpSimulation.CreatePath();
    }
}
