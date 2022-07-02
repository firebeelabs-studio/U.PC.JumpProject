using System;
using System.Collections;
using System.Collections.Generic;
using FirstGearGames.LobbyAndWorld.Lobbies.JoinCreateRoomCanvases;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    private bool _awaitingForResponseRoomCreate = false;
    private bool _awaitingForResponseRoomFind = false;
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
        MatchmakingNetwork.StartGame();
    }
}
