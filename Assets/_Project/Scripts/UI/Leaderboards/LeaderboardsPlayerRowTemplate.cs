using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Debug = ArcnesTools.Debug;

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
    [SerializeField] private Sprite _defaultBodySprite;
    [SerializeField] private Sprite _defaultHatSprite;
    [SerializeField] private Sprite _defaultEyesSprite;
    [SerializeField] private Sprite _defaultMouthSprite;
    [SerializeField] private Sprite _defaultJacketSprite;
    private const string EMPTY_NICKNAME = "NONE";
    private const string EMPTY_TIMER = "--:--";
    private Image[] _skinImages;

    public void DisplayData(LeaderboardEntry leaderboardEntry, bool updatePlaceText = false)
    {
        if (updatePlaceText)
        {
            PlaceText.text = $"{leaderboardEntry.Rank}";
        }
        NicknameText.text = leaderboardEntry.Player.Name;
        TimeText.text = DisplayTimer(leaderboardEntry.Score);
        if (!string.IsNullOrEmpty(leaderboardEntry.Metadata) && !string.IsNullOrWhiteSpace(leaderboardEntry.Metadata))
        {
            string[] skinsIds = leaderboardEntry.Metadata.Split(',');
            List<SwampieSkin> skinData = new();
            foreach (var id in skinsIds)
            {
                if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) continue;
            
                var skin = SkinsHolder.Instance.AllSkinsSO.FirstOrDefault(s => s.Id == id);
                if (skin != null)
                {
                    skinData.Add(skin);
                }
            }
            Sprite bodySprite = skinData.FirstOrDefault(o => o.skinType == SwampieSkin.SkinType.Body)?.SkinSprite;
            Sprite hatSprite = skinData.FirstOrDefault(o => o.skinType == SwampieSkin.SkinType.Hat)?.SkinSprite;
            Sprite eyesSprite = skinData.FirstOrDefault(o => o.skinType == SwampieSkin.SkinType.Eyes)?.SkinSprite;
            Sprite mouthSprite = skinData.FirstOrDefault(o => o.skinType == SwampieSkin.SkinType.Mouth)?.SkinSprite;;
            Sprite jacketSprite = skinData.FirstOrDefault(o => o.skinType == SwampieSkin.SkinType.Jacket)?.SkinSprite;
            if (bodySprite != null)
                BodyImage.sprite = bodySprite;
            if (hatSprite != null)
                HatImage.sprite = hatSprite;
            if (eyesSprite != null)
                EyesImage.sprite = eyesSprite;
            if (mouthSprite != null)
                MouthImage.sprite = mouthSprite;
            if (jacketSprite != null)
                JacketImage.sprite = jacketSprite;
        }
    }

    public void ClearRow(int index)
    {
        PlaceText.text = $"{index + 1}.";
        NicknameText.text = EMPTY_NICKNAME;
        TimeText.text = EMPTY_TIMER;
        BodyImage.sprite = _defaultBodySprite;
        HatImage.sprite = _defaultHatSprite;
        EyesImage.sprite = _defaultEyesSprite;
        MouthImage.sprite = _defaultMouthSprite;
        JacketImage.sprite = _defaultJacketSprite;
    }

    private string DisplayTimer(float time)
    {
        string minutes = Math.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        string timer = $"{minutes}:{seconds}";
        
        return timer;
    }
}
