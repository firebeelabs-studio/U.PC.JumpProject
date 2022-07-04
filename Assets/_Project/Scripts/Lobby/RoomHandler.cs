using System;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    [SerializeField] private GameObject _UI;
    private bool _awaitingForResponseRoomCreate;
    private bool _awaitingForResponseRoomFind;
    private string _cachedRoomName;

    public void ShowPls()
    {
        CheckIfThereIsRoom();
    }
    public void CheckIfThereIsRoom()
    {
         MatchmakingNetwork.CheckForAvailableRoom();
    }

    public void CreateRoom()
    {
        //TODO: block this
        //if (_awaitingForResponseRoomCreate) return;
        int playerCount = 1;
        string failedReason = String.Empty;
        //if cannot create
        if (!MatchmakingNetwork.SanitizePlayerCount(playerCount, ref failedReason))
        {
            print(failedReason);
        }
        else
        {
            _awaitingForResponseRoomCreate = true;
            MatchmakingNetwork.CreateRoom(playerCount);
            print("Created!");
        }
    }

    public void JoinRoom(string roomName)
    {
        MatchmakingNetwork.JoinRoom(roomName);
    }

    public void StartGame()
    {
        MatchmakingNetwork.StartGame();
    }

    public void HideUI()
    {
        //temp solution
        _UI.SetActive(false);
    }
}
