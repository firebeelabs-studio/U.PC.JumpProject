using SOHNE.Accessibility.Colorblindness;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetColorblindMode : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _modesDropdown;
    private void Start()
    {
        _modesDropdown.value = PlayerPrefs.GetInt("Accessibility.ColorblindType");
        _modesDropdown.onValueChanged.AddListener((modeId) =>
        {
            Colorblindness.Instance.InitChange(modeId);
            ArcnesTools.Debug.Log(modeId.ToString());
        });
    }
}
