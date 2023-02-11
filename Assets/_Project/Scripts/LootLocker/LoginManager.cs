using System.Collections;
using LootLocker.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public int PlayerId;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //if player marked
        StartCoroutine(CheckPlayerSession());
    }

    public void Login(string email, string password, bool rememberMe)
    {
        StartCoroutine(LoginRoutine(email, password, rememberMe));
    }

    public void Register(string email, string password)
    {
        StartCoroutine(RegisterRoutine(email, password));
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
                        PlayerId = response.player_id;
                        ArcnesTools.Debug.Log("session started successfully");
                        LoadingScreenCanvas.Instance.LoadScene("MainMenu");
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

    private IEnumerator RegisterRoutine(string email, string password)
    {
        bool done = false;
        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            if (response.success)
            {
                //TODO: probably we can just move player here to login panel and force him to type his data
                StartCoroutine(LoginRoutine(email, password, true));
            }
            else
            {
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    
    private IEnumerator LoginRoutine(string email, string password, bool rememberMe)
    {
        bool done = false;
        LootLockerSDKManager.WhiteLabelLogin(email, password, rememberMe, (response) =>
        {
            if (response.success)
            {
                string token = response.SessionToken;

                LootLockerSDKManager.StartWhiteLabelSession((response2) =>
                {
                    if (response2.success)
                    {
                        PlayerId = response2.player_id;
                        ArcnesTools.Debug.Log("session started successfully");
                        LoadingScreenCanvas.Instance.LoadScene("MainMenu");
                    }
                    else
                    {
                        ArcnesTools.Debug.Log("error starting LootLocker session");
                    }

                    done = true;
                });
            }
            else
            {
                //show info about failed login
                done = true;
            }
           
        });
        yield return new WaitWhile(() => done == false);
    }

}