using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsPlayerRowTemplate : MonoBehaviour
{
    public TMP_Text PlaceText;
    public Image AvatarImage;
    public TMP_Text NicknameText;
    public TMP_Text TimeText;

    public void DisplayData(int place, string nickname, Image avatar, float time)
    {
        PlaceText.text = place.ToString();
        AvatarImage.sprite = avatar.sprite;
        NicknameText.text = nickname;
    }
}
