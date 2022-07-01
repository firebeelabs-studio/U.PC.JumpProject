using System;
using System.Collections;
using System.Collections.Generic;
using FirstGearGames.LobbyAndWorld.Lobbies.JoinCreateRoomCanvases;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    private bool _awaitingForResponseRoomCreate = false;
    private bool _awaitingForResponseRoomFind = false;

    public void ShowPls()
    {
        CheckIfThereIsRoom();
        // if ()
        // {
        //     print("Here it is");
        // }
        // else
        // {
        //     print("There is no room");
        //     CreateRoom();
        // }
    }
    public void CheckIfThereIsRoom()
    {
         MatchmakingNetwork.CheckForAvailableRoom();
    }

    public void ThereIsRoom()
    {
        print("UDAŁO SIĘ");
    }
    [ContextMenu("CreateRoomMenu")]
    public void CreateRoom()
    {
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
        }
    }
}
