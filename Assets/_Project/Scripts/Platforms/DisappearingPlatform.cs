using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private DisablePlatform _disableManager;

    private void Awake()
    {
        GameManager.Platforms.Add(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _disableManager.Disable();
        }
    }
}
