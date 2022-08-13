using System;
using UnityEngine;

public class SetFlag : MonoBehaviour, ICheckpointAnim
{
    [SerializeField] Animator _anim;

    private bool _isCheckpointActivated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isCheckpointActivated) return;

        _isCheckpointActivated = true;
        _anim.Play("SetupCheckpoint");
    }
    public void ResetToDefaultState()
    {
        _isCheckpointActivated = false;
        _anim.Play("FlagDisabled");
    }
}
