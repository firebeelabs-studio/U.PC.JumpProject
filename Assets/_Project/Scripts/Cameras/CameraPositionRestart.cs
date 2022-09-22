using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionRestart : MonoBehaviour
{
    [SerializeField] private GameObject _camera;

    private void OnEnable()
    {
        FinishPanelManagement.PlayerRestart += OnPlayerRestart;
    }

    private void OnDisable()
    {
        FinishPanelManagement.PlayerRestart -= OnPlayerRestart;
        _camera.SetActive(true);
    }

    private void OnPlayerRestart()
    {
        _camera.SetActive(false);
    }
}
