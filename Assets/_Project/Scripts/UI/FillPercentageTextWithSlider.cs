using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillPercentageTextWithSlider : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    public void UpdatePercentage(float value)
    {
        if (gameObject.activeSelf)
        {
            _text.text = Mathf.RoundToInt(value * 100) + "%";
        }
    }
}
