using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class SaveScore : MonoBehaviour
{
    private Timer _timer;
    bool _isGameStarted;
    List<TimeData> Players;

    [System.Serializable]
    private class TimeData
    {
        public string WalletNumber = "Player 1";
        public int MinutesData;
        public float SecondsData;
    }
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
        if (col.gameObject.CompareTag("Player") && _isGameStarted)
        {
            _isGameStarted = false; //let the player save time only 1 time
            
            //load data from JSON and turn it into float variable that contains time in seconds
            string stringPreviousRecord = File.ReadAllText(Application.dataPath + "/scoreFile.json");
            TimeData loadPreviousRecord = JsonUtility.FromJson<TimeData>(stringPreviousRecord);
            float oldRedordInSeconds = loadPreviousRecord.MinutesData*60 + loadPreviousRecord.SecondsData;
            
            //load new time score and turn it into time in seconds
            float checkNewTime = int.Parse(_timer.minutes)*60 + float.Parse(_timer.seconds, CultureInfo.InvariantCulture.NumberFormat);

            Debug.Log("oldRecordInSeconds" + oldRedordInSeconds);
            Debug.Log("newScore" + checkNewTime);

            //check if new score is better and save, when its true;
            if (oldRedordInSeconds < checkNewTime) return;
            SaveToJson();
            Debug.Log("Congratulations! Your score is better!");
        }
    }

    public void SaveToJson()
    {
        Players = new();
        TimeData newScoreData = new TimeData();
        newScoreData.MinutesData = int.Parse(_timer.minutes);
        newScoreData.SecondsData = float.Parse(_timer.seconds, CultureInfo.InvariantCulture.NumberFormat);
        Players.Add(newScoreData);

        TimeData newScoreData2 = new TimeData();
        newScoreData2.MinutesData = int.Parse(_timer.minutes);
        newScoreData2.SecondsData = float.Parse(_timer.seconds, CultureInfo.InvariantCulture.NumberFormat);
        newScoreData2.WalletNumber = "Player 2";
        Players.Add(newScoreData2);

        string json = JsonUtility.ToJson(Players);
        File.WriteAllText(Application.dataPath + "/scoreFile.json", json);
    }
}



