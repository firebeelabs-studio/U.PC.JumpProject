using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : MonoBehaviour
{
    public void MoveCharacter(Rigidbody2D rb, Vector2 speed, float delta)
    {
        var move = speed * delta;
        rb.MovePosition(rb.position + move);
    }
    
    public float CalculateHorizontalMovement(Vector2 speed, float acceleration, float deceleration, float moveClampUpdatedEveryFrame, float inputX, float delta, bool grounded, bool colRight, bool colLeft)
    {
        if (inputX != 0)
        {
            //set horizontal move speed
            speed.x += inputX * acceleration * delta;
             
            //this clamp prevents infinitely stacking speed, it's based on frameClamp so crawling etc. won't break anything 
            speed.x = Mathf.Clamp(speed.x, -moveClampUpdatedEveryFrame, moveClampUpdatedEveryFrame);
             
            //apply bonus at the apex of a jump
            // var apexBonus = Mathf.Sign(inputX) * _apexBonus * _apexPoint;
            // speed.x += apexBonus * delta;
        }
        else
        {
            //no input slow down player using deceleration (mario like stop)
            speed.x = 0;
        }

        if (!grounded && (speed.x > 0 && colRight || speed.x < 0 && colLeft))
        {
            // Don't pile up useless horizontal (prevents sticking to walls mid-air)
            speed.x = 0;
        }
        return speed.x;
    }

    public float CalculateGravity(Vector2 speed, bool grounded, float fallClamp, bool useShortJumpFallMultiplier, float fallSpeed, float jumpEndEarlyGravityModifier, float delta)
    {
        if (grounded)
        {
            
        }
        else
        {
            //if player stopped jump faster multiply forces
            float fallSpeedCalculated = useShortJumpFallMultiplier && speed.y > 0 ? fallSpeed * jumpEndEarlyGravityModifier : fallSpeed;
            speed.y -= fallSpeedCalculated * delta;
            //hola hola amigo, don't fall too fast
            if (speed.y < fallClamp)
            {
                speed.y = fallClamp;
            }
        }

        return speed.y;
    }
}
