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
        //StartGuestSession();
    }

    public void StartGuestSession()
    {
        StartCoroutine(LoginRoutine());
    }

    private IEnumerator CheckPlayerSession()
    {
        bool done = false;
        LootLockerSDKManager.CheckWhiteLabelSession(response =>
        {
            if (response)
            {
                //somehow sessions ain't working 
                LootLockerSDKManager.StartWhiteLabelSession((response) =>
                {
                    if (response.success)
                    {
                        ArcnesTools.Debug.Log("session started successfully");
                    }
                    else
                    {
                        ArcnesTools.Debug.Log("error starting LootLocker session");
                    }
                });
                
                ArcnesTools.Debug.Log("session is valid, you can start a game session");
            }
            else
            {
                // Show login form here
                ArcnesTools.Debug.Log("session is NOT valid, we should show the login form");
            }

            done = true;
        });
        yield return new WaitWhile(() => done == false);
    }

    private IEnumerator RegisterRoutine()
    {
        string email = "xqonam@gmail.com";
        string password = "password here";
        bool done = false;
        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            if (response.success)
            {
                done = true;
            }
            else
            {
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    
    private IEnumerator LoginRoutine()
    {
        string email = "xqonam@gmail.com";
        string password = "password here";
        bool rememberMe = true;
        bool done = false;
        LootLockerSDKManager.WhiteLabelLogin(email, password, rememberMe, (response) =>
        {
            if (response.success)
            {
                string token = response.SessionToken;
                LootLockerSDKManager.StartWhiteLabelSession((response) =>
                {
                    if (response.success)
                    {
                        ArcnesTools.Debug.Log("session started successfully");
                    }
                    else
                    {
                        ArcnesTools.Debug.Log("error starting LootLocker session");
                    }
                });
            }
            else
            {
                //show info about failed login
            }
            done = true;
        });
        yield return new WaitWhile(() => done == false);
    }
}