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
        var childImg = btn.GetComponentInChildren<Image>();
        if (!childImg) return;
        
        childImg.color = Color.red;

        var childImgToReset = _currentlyActiveButton.GetComponentInChildren<Image>();
        if (!childImgToReset) return;
        
        childImgToReset.color = new Color(0, 0, 0, 0);

        _currentlyActiveButton = btn;
    }
    
}
