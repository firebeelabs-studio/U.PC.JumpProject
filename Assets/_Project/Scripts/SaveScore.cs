using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;

public class SaveScore : MonoBehaviour
{
    private Timer _timer;
    bool _isGameStarted;
    List<ScoreData> Players;
    private void Awake()
    {
        _timer = FindObjectOfType<Timer>();
    }
    private void Start()
    {
        _isGameStarted = true;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && _isGameStarted)
        {
            _isGameStarted = false; //let the player save time only 1 time
            //load data from JSON and turn it into float variable that contains time in seconds
            string stringPreviousRecord = File.ReadAllText(Application.dataPath + "/scoreFile.json");
            ScoreData loadPreviousRecord = JsonUtility.FromJson<ScoreData>(stringPreviousRecord);
            float oldRecordInSeconds = loadPreviousRecord.MinutesData * 60 + loadPreviousRecord.SecondsData;

            //load new time score and turn it into time in seconds
            float checkNewTime = int.Parse(_timer.minutes) * 60 + float.Parse(_timer.seconds, CultureInfo.InvariantCulture.NumberFormat);

            //check if new score is better and save when its true
            if (oldRecordInSeconds < checkNewTime) return;
            SaveToJson();
        }
    }
    private void SaveToJson()
    {
        Players = new();
        ScoreData newScoreData = new ScoreData();
        newScoreData.MinutesData = int.Parse(_timer.minutes);
        newScoreData.SecondsData = float.Parse(_timer.seconds, CultureInfo.InvariantCulture.NumberFormat);
        Players.Add(newScoreData);

        string json = JsonUtility.ToJson(newScoreData);
        File.WriteAllText(Application.dataPath + "/scoreFile.json", json);
    }
}



