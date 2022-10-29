using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDirection : MonoBehaviour
{
    [SerializeField] private InsectIdleMovement _insect;

    void Update()
    {
        transform.rotation = CalculateRotationBetween2Points(_insect.NewPos, transform.position);
    }

    private Quaternion CalculateRotationBetween2Points(Vector2 firstWaypoint, Vector2 secondWaypoint)
    {
        float angle;
        // calculates the vector between 2 points
        Vector2 direction = firstWaypoint - secondWaypoint;

        // fixes the angle value properly - values in inspector are different than Vector2.Angle returns
        if (firstWaypoint.y - secondWaypoint.y > 0)
        {
            angle = Vector2.Angle(Vector2.right, direction);
        }
        else
        {
            angle = -Vector2.Angle(Vector2.right, direction);
        }
        return Quaternion.Euler(0, 0, angle);
    }
}
