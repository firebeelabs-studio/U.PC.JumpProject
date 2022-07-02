using System;
using System.Collections.Generic;
using FirstGearGames.LobbyAndWorld.Clients;
using FirstGearGames.LobbyAndWorld.Lobbies.JoinCreateRoomCanvases;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Object;

public class MatchmakingNetwork : NetworkBehaviour
{
    #region Public

    //currently created rooms
    public List<RoomDetails> CreatedRooms = new List<RoomDetails>();
    //contains current room for each client
    public Dictionary<NetworkConnection, RoomDetails> ConnectionRooms = new Dictionary<NetworkConnection, RoomDetails>();
    //called when client creates a room
    public event Action<RoomDetails, NetworkObject> OnClientCreatedRoom;
    //called when client joins a room
    public event Action<RoomDetails, NetworkObject> OnClientJoinedRoom;
    //called when a member has joined your room
    public static event Action<NetworkObject> OnMemberJoined;
    public static RoomDetails CurrentRoom
    {
        get { return _instance._currentRoom; }
        private set { _instance._currentRoom = value; }
    }

    #endregion

    #region Private

    protected RoomHandler RoomHandler;
    private RoomDetails _currentRoom;
    private static MatchmakingNetwork _instance;

    #endregion

    #region Serialized

    private SearchView _searchView;

    #endregion
    
    #region Const

    private const int MINIMUM_PLAYERS_AMOUNT = 1;
    private const int MAXIMUM_PLAYERS_AMOUNT = 5;

    #endregion

    #region Initialization
    private void Awake()
    {
        _instance = this;
        RoomHandler = FindObjectOfType<RoomHandler>();
        _searchView = FindObjectOfType<SearchView>();
    }
    #endregion
    
    #region CreateRoom

    //Called on client when trying to create a room
    [Client]
    public static void CreateRoom(int playerCount)
    {
        _instance.CreateRoomInternal(playerCount);
    }

    private void CreateRoomInternal(int playerCount)
    {
        CmdCreateRoom(playerCount);
    }

    //Tries to create a room
    [ServerRpc(RequireOwnership = false)]
    private void CmdCreateRoom(int playerCount, NetworkConnection sender = null)
    {
        
        ClientInstance ci;
        if(!FindClientInstance(sender, out ci)) return;
        print("RpcCall");
        string failedReason = string.Empty;
        bool success = OnCreateRoom(playerCount, ref failedReason);

        //if nothing failed we are gucci
        if (success)
        {
            RoomDetails roomDetails = new RoomDetails("x", String.Empty, true, 5);
            roomDetails.AddMember(ci.NetworkObject);
            CreatedRooms.Add(roomDetails);
            ConnectionRooms[ci.Owner] = roomDetails;
            OnClientCreatedRoom?.Invoke(roomDetails, ci.NetworkObject);
            TargetCreateRoomSuccess(ci.Owner, roomDetails);
            RpcUpdateRooms(new[] { roomDetails });

        }
        else
        {
            //here will be added TargetRpc TargetCreateRoomFailed prob.
            print(failedReason);
        }
    }

    //Check if room can be created
    protected virtual bool OnCreateRoom(int playerCount, ref string failedReason)
    {
        if (!SanitizePlayerCount(playerCount, ref failedReason)) return false;
        if (InstanceFinder.IsServer)
        {
            //in future there will be lobby name check etc.
        }

        return true;
    }
    //Received when creating a room
    private void TargetCreateRoomSuccess(NetworkConnection ciOwner, RoomDetails roomDetails)
    {
        CurrentRoom = roomDetails;
        print("Room created");
        //send member joined to self
        MemberJoined(InstanceFinder.ClientManager.Connection.FirstObject);
    }

    public static bool SanitizePlayerCount(int count, ref string failedReason)
    {
        return _instance.OnSanitizePlayerCount(count, ref failedReason);
    }
    
    //Check if number of players in room is valid
    protected virtual bool OnSanitizePlayerCount(int count, ref string failedReason)
    {
        if (count < OnReturnMinimumPlayersAmount() || count > OnReturnMaximumPlayersAmount())
        {
            failedReason = "Invalid number of players.";
            return false;
        }

        return true;
    }
    
    public static int ReturnMinimumPlayersAmount()
    {
       return _instance.OnReturnMinimumPlayersAmount();
    }
    protected virtual int OnReturnMinimumPlayersAmount()
    {
        return MINIMUM_PLAYERS_AMOUNT;
    }

    public static int ReturnMaximumPlayersAmount()
    {
        return _instance.OnReturnMaximumPlayersAmount();
    }
    protected virtual int OnReturnMaximumPlayersAmount()
    {
        return MAXIMUM_PLAYERS_AMOUNT;
    }

    #endregion
    
    #region Helpers

    //Find and out  the ClientInstance for a connection
    private bool FindClientInstance(NetworkConnection conn, out ClientInstance ci)
    {
        ci = null;
        if (conn is null)
        {
            print("Connection is null. We are doomed");
            return false;
        }

        ci = ClientInstance.ReturnClientInstance(conn);
        if (ci is null)
        {
            print("ClientInstance not found for connection :(");
            return false;
        }

        return true;
    }

    private RoomDetails ReturnRoomDetails(string roomName)
    {
        for (int i = 0; i < CreatedRooms.Count; i++)
        {
            if (CreatedRooms[i].Name.Equals(roomName, StringComparison.CurrentCultureIgnoreCase))
            {
                return CreatedRooms[i];
            }
        }

        return null;
    }
    
    //Find room by NetworkObject
    private RoomDetails ReturnRoomDetails(NetworkObject clientId)
    {
        for (int i = 0; i < CreatedRooms.Count; i++)
        {
            for (int j = 0; j < CreatedRooms[i].MemberIds.Count; j++)
            {
                if (CreatedRooms[i].MemberIds[j] == clientId)
                {
                    return CreatedRooms[i];
                }
            }
        }
        return null;
    }

    #endregion
    
    #region Manage Rooms
    
    //Updates rooms on all clients. Even these ones in matches. We can ignore that till we will make normal lobby system i guess
    [ObserversRpc]
    public void RpcUpdateRooms(RoomDetails[] roomDetails)
    {
        //Check if CurrentRoom needs to be updated
        if (CurrentRoom is not null)
        {
            for (int i = 0; i < roomDetails.Length; i++)
            {
                if (roomDetails[i].Name == CurrentRoom.Name)
                {
                    CurrentRoom = roomDetails[i];
                    break;
                }
            }
        }

        if (CurrentRoom is not null)
        {
            _searchView.PlayersCount = CurrentRoom.MemberIds.Count;
        }
    }


    [Client]
    public static void CheckForAvailableRoom()
    {
        _instance.CheckForAvailableRoomInternal();
    }

    private void CheckForAvailableRoomInternal()
    { 
        CmdCheckForAvailableRoom();
    }
    //probably it don't have to be server side, we will see
    [ServerRpc(RequireOwnership = false)]
    private void CmdCheckForAvailableRoom(NetworkConnection sender = null)
    {
        ClientInstance ci;
        if (!FindClientInstance(sender, out ci))
            return;
        
        //if someone is already in room - have to move this out to helpers prob, cuz rn we will create new room in this case
        //if (CurrentRoom is not null) return;
        
        //if there are no rooms
        if (CreatedRooms.Count == 0)
        {
            TargetSendRoomsFailure(ci.Owner);
            return;
        }
        
        foreach (var room in CreatedRooms)
        {
            //if is full
            if (room.MemberIds.Count >= room.MaxPlayers) continue;
            //if game already started
            if (room.IsStarted && room.LockOnStart) continue;
            
            //join room
            TargetSendRooms(ci.Owner, room.Name);
            return;
        }
        //rooms are full, create new one
        TargetSendRoomsFailure(ci.Owner);
    }

    [TargetRpc]
    private void TargetSendRooms(NetworkConnection conn, string roomName)
    {
        RoomHandler.JoinRoom(roomName);
    }
    
    [TargetRpc]
    private void TargetSendRoomsFailure(NetworkConnection conn)
    {
        RoomHandler.CreateRoom();
    }

    #endregion
    
    #region JoinRoom

    [Client]
    public static void JoinRoom(string roomName)
    {
        _instance.JoinRoomInternal(roomName);
    }

    private void JoinRoomInternal(string roomName)
    {
        CmdJoinRoom(roomName);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void CmdJoinRoom(string roomName, NetworkConnection sender = null)
    {
        ClientInstance ci;
        
        if (!FindClientInstance(sender, out ci)) return;

        string failedReason = string.Empty;
        RoomDetails roomDetails = null;
        bool success = OnJoinRoom(roomName, ci.NetworkObject, ref failedReason, ref roomDetails);
        if (success)
        {
            roomDetails.AddMember(ci.NetworkObject);
            ConnectionRooms[ci.Owner] = roomDetails;
            TargetJoinRoomSuccess(ci.Owner, roomDetails);
            OnClientJoinedRoom?.Invoke(roomDetails, ci.NetworkObject);

            RpcUpdateRooms(new[] { roomDetails });
            foreach (NetworkObject item in roomDetails.MemberIds)
            {
                TargetMemberJoined(item.Owner, ci.NetworkObject);
            }
        }
        else
        {
            TargetJoinRoomFailed(ci.Owner, failedReason);
        }
    }

    private bool OnJoinRoom(string roomName, NetworkObject joiner, ref string failedReason, ref RoomDetails roomDetails)
    {
        if (ReturnRoomDetails(joiner) is not null)
        {
            failedReason = "You are already in a room.";
            return false;
        }

        roomDetails = ReturnRoomDetails(roomName);
        if (roomDetails is null)
        {
            failedReason = "Room does not exist.";
            return false;
        }
        else
        {
            //is full
            if (roomDetails.MemberIds.Count >= roomDetails.MaxPlayers)
            {
                failedReason = "Room is full,";
                return false;
            }
            //is started
            if (roomDetails.IsStarted && roomDetails.LockOnStart)
            {
                failedReason = "Room has already started.";
                return false;
            }
        }

        return true;
    }
    
    [TargetRpc]
    private void TargetJoinRoomSuccess(NetworkConnection conn, RoomDetails roomDetails)
    {
        CurrentRoom = roomDetails;
    }

    [TargetRpc]
    private void TargetMemberJoined(NetworkConnection conn, NetworkObject member)
    {
        if (CurrentRoom is null) return;

        MemberJoined(member);
    }

    [TargetRpc]
    private void TargetJoinRoomFailed(NetworkConnection conn, string failedReason)
    {
        CurrentRoom = null;
    }

    private void MemberJoined(NetworkObject member)
    {
        CurrentRoom.AddMember(member);
        OnMemberJoined?.Invoke(member);
    }
    
    #endregion
}
