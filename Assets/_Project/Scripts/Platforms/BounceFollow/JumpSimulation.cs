using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSimulation : MonoBehaviour
{
    [SerializeField] private GameObject _bouncer;
    [SerializeField] private GameObject _playerSimulation;
    [SerializeField] private GameObject _collisionDetector;
    [SerializeField] private PathFollower _pathFollower;
    [SerializeField] private Rigidbody2D _rb;

    public bool isSimulationActive;

    public void CreatePath() // activates the path creator - "fake" player respawns in the middle of the bouncer, jump and draw a path
    {
        _pathFollower.ResetDrawing();
        transform.position = new Vector2(_bouncer.transform.position.x, _bouncer.transform.position.y);

        //changing the rotation changes the size of collider
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _collisionDetector.transform.rotation = Quaternion.Euler(0, 0, 0);
        _playerSimulation.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision) // "fake" player disappear on collision
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _playerSimulation.SetActive(false);
        }
    }

    void OnEnable()
    {
        _rb.velocity = Vector2.zero;
        isSimulationActive = true;
    }

    void OnDisable()
    {
        _rb.velocity = Vector2.zero;
        isSimulationActive = false;
    }
}


