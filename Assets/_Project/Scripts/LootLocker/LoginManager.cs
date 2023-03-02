using System.Collections;
using LootLocker.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public int PlayerId;
    public string Nick;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //if player marked
        CheckPlayerSession();
    }

    public void Login(string email, string password, bool rememberMe)
    {
        StartCoroutine(LoginRoutine(email, password, rememberMe));
    }

    public void Register(string email, string password, string playerName)
    {
        StartCoroutine(RegisterRoutine(email, password, playerName));
    }

    public void CheckPlayerSession()
    {
        StartCoroutine(CheckPlayerSessionRoutine());
    }
    private IEnumerator CheckPlayerSessionRoutine()
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

    private IEnumerator RegisterRoutine(string email, string password, string playerName)
    {
        bool done = false;
        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            if (response.success)
            {
                LootLockerSDKManager.WhiteLabelLogin(email, password, true, (response3) =>
                {
                    if (response.success)
                    {
                        string token = response3.SessionToken;

                        LootLockerSDKManager.StartWhiteLabelSession((response2) =>
                        {
                            if (response2.success)
                            {
                                PlayerId = response2.player_id;
                                
                                LootLockerSDKManager.SetPlayerName(playerName, (nameChangeResponse) =>
                                {
                                    if (nameChangeResponse.success)
                                    {
                                        Nick = playerName;
                                        ArcnesTools.Debug.Log("Account created successfully");
                                        done = true;
                                        LoadingScreenCanvas.Instance.LoadScene("MainMenu");
                                    }
                                    else
                                    {
                                        ArcnesTools.Debug.Log("Name change failed");
                                        done = true;
                                    }
                                });
                            }
                            else
                            {
                                ArcnesTools.Debug.Log("error starting LootLocker session");
                                done = true;
                            }
                            
                        });
                    }
                    else
                    {
                        //show info about failed login
                        done = true;
                    }
                });
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