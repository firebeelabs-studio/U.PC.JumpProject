using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class ActivateSkill : MonoBehaviour
{
    private void Start()
    {
        GameManager.Collectibles.Add(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().AllowDoubleJump = true;
            gameObject.SetActive(false);
        }
    }
}
