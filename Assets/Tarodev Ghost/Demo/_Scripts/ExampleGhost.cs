using UnityEngine;

namespace Tarodev {
    public class ExampleGhost : MonoBehaviour {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private GameObject _poofPrefab;
        private float _lastXPos;

        private void Awake() => _renderer = GetComponentInChildren<SpriteRenderer>();

        private void OnEnable() {
            _lastXPos = transform.position.x;
            FinishLine.Crossed += FinishLineOnCrossed;
        }

        private void OnDisable() => FinishLine.Crossed -= FinishLineOnCrossed;

        private void FinishLineOnCrossed(bool running) {
            // If this triggers before the ghost is destroyed, the player beat the ghost
            if (!running) Instantiate(_poofPrefab, transform.position, Quaternion.identity);
        }

        private void Update() => FaceCorrectDirection(transform.position.x);

        private void FaceCorrectDirection(float xPos) {
            if (xPos > _lastXPos) {
                _renderer.flipX = false;
                _lastXPos = xPos;
            }
            else if (xPos < _lastXPos) {
                _renderer.flipX = true;
                _lastXPos = xPos;
            }
        }
    }
}