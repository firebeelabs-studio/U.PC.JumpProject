using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonsGroup : MonoBehaviour
{
    [SerializeField] private Button _currentlyActiveButton;
    
    public void OnClick(Button btn)
    {
        var childImg = btn.transform.GetChild(0).GetComponent<Image>();
        if (!childImg) return;
        
        childImg.color = Color.red;
        if (_currentlyActiveButton)
        {
            var childImgToReset = _currentlyActiveButton.transform.GetChild(0).GetComponent<Image>();
            if (!childImgToReset) return;

            childImgToReset.color = Color.white;
        }
        
        _currentlyActiveButton = btn;
    }
    
}
