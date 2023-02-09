using System.Collections;
using LootLocker.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _nicknameText;
    [SerializeField] private TMP_Text _changedNicknameText;
    [SerializeField] private GameObject _nameSetPanel;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartGuestSession();
    }

    public void StartGuestSession()
    {
        StartCoroutine(LoginRoutine());
    }

    public void SetPlayerName()
    {
        PlayerPrefs.SetString("NicknameSet", "true");
        LootLockerSDKManager.SetPlayerName(_nicknameText.text, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Name set");
            }
            else
            {
                Debug.Log("Error");
            }
        });
    }

    public void ChangePlayerName()
    {
        PlayerPrefs.SetString("NicknameSet", "true");
        LootLockerSDKManager.SetPlayerName(_changedNicknameText.text, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Name set");
            }
            else
            {
                Debug.Log("Error");
            }
        });
    }
    
    private IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                PlayerPrefs.SetString("PlayerId", response.player_id.ToString());
                done = true;
                if (PlayerPrefs.GetString("NicknameSet") is not null && PlayerPrefs.GetString("NicknameSet") == "true")
                {
                    //SceneManager.LoadScene("EvoWorld");
                    return;
                }
                else
                {
                    _nameSetPanel.SetActive(true);
                }
            }
            else
            {
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}