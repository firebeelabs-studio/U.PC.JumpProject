using UnityEngine;

public class SetCameraAsScreenSpace : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    private Camera _camera;
    void Awake()
    {
        _camera = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        _canvas.worldCamera = _camera;
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
    }
}
