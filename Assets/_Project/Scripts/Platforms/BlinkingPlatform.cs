using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingPlatform : MonoBehaviour
{
    [SerializeField] private float _visibleDuration;
    [SerializeField] private float _invisibleDuration;
    private SpriteRenderer _sprite;
    private BoxCollider2D _collider;
    private float _timer;
    private bool _shouldBeVisible;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        _shouldBeVisible = true;
        if (_visibleDuration < 0)
        {
            _visibleDuration *= -1;
        }
        if (_invisibleDuration < 0)
        {
            _invisibleDuration *= -1;
        }
    }
    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_shouldBeVisible)
        {
            ChangeState(_invisibleDuration, false, true); //appear
        }
        else
        {
            ChangeState(_visibleDuration, true, false); //disappear
        }
    }
    private void ChangeState(float nextVisibilityDuration, bool shouldBeVisible, bool shouldEnableColliderAndSprite)
    {
        _collider.enabled = shouldEnableColliderAndSprite;
        _sprite.enabled = shouldEnableColliderAndSprite;
        if (_timer <= 0)
        {
            _shouldBeVisible = shouldBeVisible;
            _timer = nextVisibilityDuration;
        }
    }
}
