using System.Collections;
using LootLocker.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public int PlayerId;
    public string Nick;
    
    public static LoginManager Instance { get; private set; }
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
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
                LootLockerSDKManager.StartWhiteLabelSession((response) =>
                {
                    if (response.success)
                    {
                        PlayerId = response.player_id;
                        ArcnesTools.Debug.Log("session started successfully");
                        LootLockerSDKManager.GetPlayerName((getPlayerNameResponse) =>
                        {
                            if (getPlayerNameResponse.success)
                            {
                                Nick = getPlayerNameResponse.name;
                            } 
                            LoadingScreenCanvas.Instance.LoadScene("MainMenu");
                            done = true;
                        });
                    }
                    else
                    {
                        ArcnesTools.Debug.Log("error starting LootLocker session");
                        done = true;
                    }
                });
                
                ArcnesTools.Debug.Log("session is valid, you can start a game session");
            }
            else
            {
                // Show login form here
                ArcnesTools.Debug.Log("session is NOT valid, we should show the login form");
                done = true;
            }

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
                LootLockerSDKManager.WhiteLabelLoginAndStartSession(email, password, true, (response3) =>
                {
                    if (response3.success)
                    {
                        PlayerId = response3.SessionResponse.player_id;
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
        LootLockerSDKManager.WhiteLabelLoginAndStartSession(email, password, rememberMe, (response) =>
        {
            if (response.success)
            {
                PlayerId = response.SessionResponse.player_id;
                ArcnesTools.Debug.Log("session started successfully");
                LootLockerSDKManager.GetPlayerName((getPlayerNameResponse) =>
                {
                    if (getPlayerNameResponse.success)
                    {
                        Nick = getPlayerNameResponse.name;
                    } 
                    done = true;
                    LoadingScreenCanvas.Instance.LoadScene("MainMenu");
                });

            }
            else
            {
                //show info about failed login
                done = true;
                ArcnesTools.Debug.Log("error starting LootLocker session");
            }
           
        });
        yield return new WaitWhile(() => done == false);
    }

}