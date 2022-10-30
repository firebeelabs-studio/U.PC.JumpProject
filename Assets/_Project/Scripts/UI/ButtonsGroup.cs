using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonsGroup : MonoBehaviour
{
    [SerializeField] private AudioSource _UIAudioSource;
    [SerializeField] private Button _currentlyActiveButton;
    
    public void OnClick(Button btn)
    {
        if (btn == _currentlyActiveButton) return;

        var childToEnable = btn.transform.GetChild(1).gameObject;
        if (!childToEnable) return;

        childToEnable.SetActive(true);
        
        if (_currentlyActiveButton)
        {
            var childToDisable = _currentlyActiveButton.transform.GetChild(1).gameObject;
            if (!childToDisable) return;

            childToDisable.SetActive(false);
        }
        
        _currentlyActiveButton = btn;
        _UIAudioSource.Play();
    }
    
}
