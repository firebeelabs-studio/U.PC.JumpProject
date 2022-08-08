using UnityEngine;
using TarodevController;
using System;

public class BouncePlatform : MonoBehaviour
{
    [SerializeField] private float _bounceForce = 20;
    [SerializeField] private float _horizontalBoost = 1;
    [SerializeField] private float _cameraZoomOutDuration;
    //[SerializeField] private JumpSimulation _jumpSimulation;
   
    private Vector2 _bounceDirectionVector;
    private bool _cancelMovement = true;
    private float sinDegree;
    private float cosDegree;

    public float Vx => sinDegree * _bounceForce * _horizontalBoost;
    public float Vy => cosDegree * _bounceForce;

    void Awake()
    {
        CalculateForces();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (collision.TryGetComponent(out IPlayerController controller))
        {
            controller.ApplyVelocity(_bounceDirectionVector * _bounceForce, PlayerForce.Burst);
        }
    }

    void CalculateForces()
    {
        float degreeInRadians = (transform.transform.eulerAngles.z * (Mathf.PI)) / 180;
        sinDegree = Mathf.Sin(degreeInRadians);
        cosDegree = Mathf.Cos(degreeInRadians);
        _bounceDirectionVector = new Vector2(-sinDegree * _horizontalBoost, cosDegree);
    }

    [ContextMenu("Create Path")]
    void SetPath()
    {
        CalculateForces();
        //_jumpSimulation.CreatePath();
    }
}
