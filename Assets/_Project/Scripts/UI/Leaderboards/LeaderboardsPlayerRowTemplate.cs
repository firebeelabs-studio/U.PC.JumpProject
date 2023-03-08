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
    private const string EMPTY_NICKNAME = "NONE";
    private const string EMPTY_TIMER = "--:--";

    public void DisplayData(LeaderboardEntry leaderboardEntry)
    {
        PlaceText.text = $"{leaderboardEntry.Rank}.";
        NicknameText.text = leaderboardEntry.Player.Name;
        TimeText.text = DisplayTimer(leaderboardEntry.Score);
    }

    public void ClearRow(int index)
    {
        PlaceText.text = $"{index + 1}";
        NicknameText.text = EMPTY_NICKNAME;
        TimeText.text = EMPTY_TIMER;
    }

    private string DisplayTimer(float time)
    {
        string minutes = Math.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        string timer = $"{minutes}:{seconds}";
        
        return timer;
    }
}
