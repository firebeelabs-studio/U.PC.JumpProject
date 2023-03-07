using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class SetNicknameInMenu : MonoBehaviour
{
    private TMP_Text _nickname;

    private void Awake()
    {
        _nickname = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _nickname.text = LoginManager.Instance.Nick;
    }
}
