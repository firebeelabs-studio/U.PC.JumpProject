using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<GameObject> Collectibles = new List<GameObject>();
    public static List<GameObject> Platforms = new List<GameObject>();
    public static PlayerController Player = new PlayerController();
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
}
