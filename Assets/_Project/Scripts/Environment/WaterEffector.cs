using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffector : MonoBehaviour
{
    private float _previousAcceleration;
    private float _previousMoveClamp;
    private float _previousDeceleration;
    private float _previousFallClamp;
    private float _previousJumpHeight;
    private float _previousJumpApexThreshold;
    private float _previousGroundingForce;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PawnController pawnController))
            {
                SavePreviousValues(pawnController.Acceleration, pawnController.Deleceration, pawnController.MoveClamp, pawnController.FallClamp, pawnController.GroundingForce, pawnController.JumpHeight, pawnController.JumpApexThreshold);
                pawnController.ToggleUnderwaterBehaviour();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PawnController pawnController))
            {
                pawnController.ToggleUnderwaterBehaviour(_previousAcceleration, _previousDeceleration, _previousMoveClamp, _previousFallClamp , _previousGroundingForce, _previousJumpHeight, _previousJumpApexThreshold);
            }
        }
    }
    private void SavePreviousValues(float acceleration, float deceleration, float moveClamp, float fallClamp, float groundingForce, float jumpHeight, float jumpApexThreshold)
    {
        _previousAcceleration = acceleration;
        _previousDeceleration = deceleration;
        _previousMoveClamp = moveClamp;
        _previousFallClamp = fallClamp;
        _previousGroundingForce = groundingForce;
        _previousJumpHeight = jumpHeight;
        _previousJumpApexThreshold = jumpApexThreshold;
    }

}
