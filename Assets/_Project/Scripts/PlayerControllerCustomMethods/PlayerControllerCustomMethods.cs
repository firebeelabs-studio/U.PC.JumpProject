using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TarodevController
{
    public partial class PlayerController : NetworkBehaviour, IPlayerController
    {
        public void AddForce(Vector2 force, PlayerForce mode = PlayerForce.Burst, bool cancelMovement = true)
        {
            if (cancelMovement) _speed = Vector2.zero;

            switch (mode)
            {
                case PlayerForce.Burst:
                    _speed += force;
                    break;
                case PlayerForce.Decay:
                    _forceBuildup += force * Time.fixedDeltaTime;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
        public void ChangeMoveClamp(float slowPower)
        {
            _moveClamp -= slowPower;
        }
        public void SetBoosts(float accBoost, float clampBoost, float decBoost)
        {
            _acceleration += accBoost;
            _moveClamp += clampBoost;
            _deceleration += decBoost;
        }
        public void MudDebuff(float accDebuff, float jumpDebuff, float clampDebuff)
        {
            _acceleration -= accDebuff;
            _jumpHeight -= jumpDebuff;
            _moveClamp -= clampDebuff;
        }
        public void IceDebuff(float accDebuff, float decDebuff)
        {
            _acceleration -= accDebuff;
            _deceleration -= decDebuff;
        }
    }
}

