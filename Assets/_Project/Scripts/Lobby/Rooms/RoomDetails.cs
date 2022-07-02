using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomDetails
{
    public RoomDetails() { }

    public RoomDetails(string name, string password, bool lockOnStart, int maxPlayers)
    {
        Name = name;
        Password = password;
        IsPassworded = !string.IsNullOrEmpty(password);
        LockOnStart = lockOnStart;
        MaxPlayers = maxPlayers;
    }

    public string Name;
    public int MaxPlayers;
    public bool IsStarted;
    public bool IsPassworded;
    [NonSerialized] public string Password = string.Empty;
    public bool LockOnStart;
    [NonSerialized] public HashSet<Scene> Scenes = new();
    public List<NetworkObject> MemberIds = new List<NetworkObject>();
    public List<NetworkObject> StartedMembers = new List<NetworkObject>();
    [NonSerialized] public List<NetworkObject> KickedIds = new List<NetworkObject>();

    internal void AddMember(NetworkObject clientId)
    {
        if (!MemberIds.Contains(clientId))
        {
            MemberIds.Add(clientId);
        }
    }

    internal void AddStartedMember(NetworkObject clientId)
    {
        if (!StartedMembers.Contains(clientId))
        {
            StartedMembers.Add(clientId);
        }
    }

    internal void ClearMembers()
    {
        MemberIds.Clear();
    }

    internal bool IsKickedMember(NetworkObject clientId)
    {
        return KickedIds.Contains(clientId);
    }

    //kick user from room
    internal void AddKicked(NetworkObject clientId)
    {
        if (IsKickedMember(clientId)) return;
        
        KickedIds.Add(clientId);
    }
}
