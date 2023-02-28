using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsPlayerRowTemplate : MonoBehaviour
{
    public TMP_Text PlaceText;
    public Image BodyImage;
    public Image HatImage;
    public Image EyesImage;
    public Image MouthImage;
    public Image JacketImage;
    public TMP_Text NicknameText;
    public TMP_Text TimeText;

    public void DisplayData(LeaderboardEntry leaderboardEntry)
    {
        PlaceText.text = $"{leaderboardEntry.Rank}.";
        NicknameText.text = leaderboardEntry.Player.Name;
        TimeText.text = DisplayTimer(leaderboardEntry.Score);
    }

    private string DisplayTimer(float time)
    {
        string minutes = Math.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        string timer = $"{minutes}:{seconds}";
        
        return timer;
    }
}
