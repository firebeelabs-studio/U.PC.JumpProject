using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class GuillotineObstacle : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _delay;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _distance;
    [Space(10)]
    [Header("PRESS")]
    [SerializeField] private GameObject _press;
    [SerializeField] private ParticleSystem _pressParticles;
    private AudioPlayer _audioPlayer;
    private Vector2 _startPos;
    private float _angle, _timer;
    [Space(10)]
    [Header("Sounds")]
    [SerializeField] private AudioClip _smashSound;

    [SerializeField] private float _slowDuration;
    private Animator[] _animators;
    private float _distanceBetweenPressOriginAndPawn;
    private bool _canPress = false;


    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Start()
    {
        _startPos = transform.position;
        _timer = _startDelay;
    }
    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _angle += Time.deltaTime * _speed;
            MoveObstacle(_press);
        }
        if (_canPress)
        {
            _animators.Last().Play("Press");
            _canPress = false;
        }
    }
    private void MoveObstacle(GameObject obstacle)
    {
        obstacle.transform.position = new Vector2(_startPos.x, _startPos.y + Mathf.Sin(_angle) * _distance);

        if (_angle >= 2 * Mathf.PI)
        {
            _angle = 0;
            _timer = _delay;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.TryGetComponent(out PawnController pawnController))
            {
                if (pawnController.Grounded && _angle >= Mathf.PI)
                {
                    _animators = pawnController.GetComponentsInChildren<Animator>();
                    _distanceBetweenPressOriginAndPawn = Vector2.Distance(_press.transform.position, pawnController.transform.position);
                }
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && _angle >= Mathf.PI)
        {
            _pressParticles.Play();
            _audioPlayer.PlayOneShotSound(_smashSound);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PawnController pawnController))
            {
                if (pawnController.Grounded && _angle >= Mathf.PI)
                {
                    float newDistance = Vector2.Distance(_press.transform.position, pawnController.transform.position);
                    if (newDistance < _distanceBetweenPressOriginAndPawn && !_canPress)
                    {
                        _canPress = true;

                    }
                }
            }
        }
    }

    private IEnumerator SlowDownPlayer(PawnController pawnController)
    {
        //pawnController.
        yield return new WaitForSeconds(_slowDuration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -_distance));
    }
}
