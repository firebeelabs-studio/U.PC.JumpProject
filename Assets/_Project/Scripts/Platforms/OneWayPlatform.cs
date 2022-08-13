using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private BoxCollider2D _col;
    private IPawnController _controller;

    [SerializeField] private float _fallThroughUnlockTime = 0.25f;

    private float _timeToUnlock = float.MinValue;
    private void Awake() => _col = GetComponent<BoxCollider2D>();

    private void Update() {
        if (_controller == null) return;
        if (_controller.Input.Move.y < 0) _timeToUnlock = Time.time + _fallThroughUnlockTime;
        //_controller.ForceBuildup.y <= 0.7f adding this allows using it even with using decoy on bouncers
        _col.enabled = _controller.RawMovement.y <= 0 && Time.time >= _timeToUnlock;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent(out IPawnController controller)) {
            _controller = controller;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.TryGetComponent(out IPawnController controller)) _controller = null;
    }
}
