using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInput : MonoBehaviour
{
    private PawnMoveData _moveData;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _moveData.Jump = true;
        }
    }

    public PawnMoveData ConsumeData()
    {
        ReadFrameInput();
        PawnMoveData data = _moveData;
        ResetData();
        return data;
    }
    
    private void ReadFrameInput()
    {
        _moveData.Horizontal = Input.GetAxisRaw("Horizontal");
    }

    private void ResetData()
    {
        _moveData.Jump = false;
        _moveData.Horizontal = 0f;
    }
}
