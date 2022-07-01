using System;
using System.Collections;
using System.Collections.Generic;
using FirstGearGames.LobbyAndWorld.Lobbies.JoinCreateRoomCanvases;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    private bool _awaitingForResponse = false;

    public void CheckIfThereIsRoom()
    {
        
    }
    [ContextMenu("CreateRoomMenu")]
    public void CreateRoom()
    {
        if (_awaitingForResponse) return;
        int playerCount = 1;
        string failedReason = String.Empty;
        //if cannot create
        if (!MatchmakingNetwork.SanitizePlayerCount(playerCount, ref failedReason))
        {
            print(failedReason);
        }
        else
        {
            _awaitingForResponse = true;
            MatchmakingNetwork.CreateRoom(playerCount);
        }
    }
}
