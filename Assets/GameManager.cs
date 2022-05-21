using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<GameObject> Collectibles = new List<GameObject>();

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
    }

    public static void SpawnAllCollectibles()
    {
        Collectibles.ForEach(x => x.SetActive(true));
    }
}
