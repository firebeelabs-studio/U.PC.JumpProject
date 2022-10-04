using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFlip : MonoBehaviour
{
    private Vector2 movement;

    private void Update()
    {
        movement = transform.position;
        bool _flipped = movement.x < 0;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, _flipped ? 180f : 0f, 0f));
    }
}