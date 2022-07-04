using UnityEngine;
using TarodevController;

public class BouncePlatform : MonoBehaviour
{
    [SerializeField] private float _bounceForce = 20;
    [SerializeField] private float _horizontalBoost = 1;
    [SerializeField] private float _cameraZoomOutDuration;
    private Vector2 _bounceDirectionVector;
    private bool _cancelMovement = true;

    private void Start()
    {
            float degreeInRadians = (transform.transform.eulerAngles.z * (Mathf.PI)) / 180;
            float sinDegree = Mathf.Sin(degreeInRadians);
            float cosDegree = Mathf.Cos(degreeInRadians);
            _bounceDirectionVector = new Vector2(-sinDegree*_horizontalBoost, cosDegree);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (collision.TryGetComponent(out IPlayerController controller))
        {
            controller.AddForce(_bounceDirectionVector * _bounceForce, PlayerForce.Burst, _cancelMovement);
        }
    }
}
