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
    
    public Vector2 CalculateHorizontalMovement(Vector2 speed, float acceleration, float deceleration, float moveClampUpdatedEveryFrame, float inputX, float delta)
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

        return speed;
        // if (!_grounded && (speed.x > 0 && _colRight || speed.x < 0 && _colLeft))
        // {
        //     // Don't pile up useless horizontal (prevents sticking to walls mid-air)
        //     speed.x = 0;
        // }
    }
}
