using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimatorNetworking : MonoBehaviour
{
    private Animator _animator;
    private PlayerMotor _player;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _player = GetComponentInParent<PlayerMotor>();
    }

    public void SetMoving(bool value)
    {
        _animator.SetBool("Moving", value);
    }

    public void Jump()
    {
        _animator.SetTrigger("Jump");
    }

    private void Update()
    {
        //sprite flip
        if (_player.Input.X != 0)
        {
            //works only local :((
            transform.localScale = new Vector3(_player.Input.X > 0 ? 1.3f : -1.3f, transform.localScale.y, transform.localScale.z);
        }
    }
}
