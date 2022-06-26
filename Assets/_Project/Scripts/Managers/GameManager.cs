using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TarodevController;
using UnityEngine;

public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    public static List<GameObject> Collectibles = new List<GameObject>();
    public static List<GameObject> Platforms = new List<GameObject>();
    public static PlayerController Player = new PlayerController();
    
    [field: SyncObject] public readonly SyncList<User> Users = new SyncList<User>();

    [field: SyncVar] public bool CanStart { get; private set; }

    public GameObject TestObj;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!IsServer) return;
        Instance.CanStart = !Instance.Users.Any(p => p.IsReady == false);
    }

    private void OnEnable()
    {
        FinishLevel.EndRun += EndRun;
        StartRun.RunStart += EndRun;
    }

    private void OnDisable()
    {
        FinishLevel.EndRun -= EndRun;
        StartRun.RunStart -= EndRun;
    }

    public void EndRun()
    {
        SpawnAllCollectibles();
        SpawnAllPlatforms();
    }
    
    public static void SpawnAllCollectibles()
    {
        Collectibles.ForEach(x => x.SetActive(true));
    }
    public static void SpawnAllPlatforms()
    {
        Platforms.ForEach(x => x.SetActive(true));
    }
    public static void ResetPlayerPowers()
    {
        Player.AllowDoubleJump = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartGame()
    {
        FindObjectOfType<SceneLoader>().LoadScene();
    }
    
}
