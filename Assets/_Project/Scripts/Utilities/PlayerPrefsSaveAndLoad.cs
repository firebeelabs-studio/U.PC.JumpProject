using System;
using UnityEngine;

public static class PlayerPrefsSaveAndLoad
{
    private const string RememberMeToggle = "RememberMeToggle";
    private const string LastUsedHat = "LastUsedHat";
    private const string LastUsedEyes = "LastUsedEyes";
    private const string LastUsedMouth = "LastUsedMouth";
    private const string LastUsedJacket = "LastUsedJacket";
    private const string LastUsedBody = "LastUsedBody";
    
    public static bool LoadRememberMeToggle()
    {
        if (!PlayerPrefs.HasKey(RememberMeToggle))
        {
            SaveRememberMeToggle(0);
        }
        return PlayerPrefs.GetInt(RememberMeToggle) == 1;
    }

    public static void SaveRememberMeToggle(int value)
    {
        PlayerPrefs.SetInt(RememberMeToggle, value);
    }

    public static void SaveLastUsedSkin(SwampieSkin.SkinType skinType, string id)
    {
        switch (skinType)
        {
            case SwampieSkin.SkinType.Hat:
                PlayerPrefs.SetString(LastUsedHat, id);
                break;
            case SwampieSkin.SkinType.Jacket:
                PlayerPrefs.SetString(LastUsedJacket, id);
                break;
            case SwampieSkin.SkinType.Eyes:
                PlayerPrefs.SetString(LastUsedEyes, id);
                break;
            case SwampieSkin.SkinType.Mouth:
                PlayerPrefs.SetString(LastUsedMouth, id);
                break;
            case SwampieSkin.SkinType.Body:
                PlayerPrefs.SetString(LastUsedBody, id);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(skinType), skinType, null);
        }
    }

    public static string LoadLastUsedSkin(SwampieSkin.SkinType skinType)
    {
        switch (skinType)
        {
            case SwampieSkin.SkinType.Hat:
                if (!PlayerPrefs.HasKey(LastUsedHat))
                {
                    SaveLastUsedSkin(SwampieSkin.SkinType.Hat, "");
                }
                return PlayerPrefs.GetString(LastUsedHat);
            
            case SwampieSkin.SkinType.Jacket:
                if (!PlayerPrefs.HasKey(LastUsedJacket))
                {
                    SaveLastUsedSkin(SwampieSkin.SkinType.Jacket, "");
                }
                return PlayerPrefs.GetString(LastUsedJacket);
            
            case SwampieSkin.SkinType.Eyes:
                if (!PlayerPrefs.HasKey(LastUsedEyes))
                {
                    SaveLastUsedSkin(SwampieSkin.SkinType.Eyes, "");
                }
                return PlayerPrefs.GetString(LastUsedEyes);
            
            case SwampieSkin.SkinType.Mouth:
                if (!PlayerPrefs.HasKey(LastUsedMouth))
                {
                    SaveLastUsedSkin(SwampieSkin.SkinType.Mouth, "");
                }
                return PlayerPrefs.GetString(LastUsedMouth);
            
            case SwampieSkin.SkinType.Body:
                if (!PlayerPrefs.HasKey(LastUsedBody))
                {
                    SaveLastUsedSkin(SwampieSkin.SkinType.Body, "");
                }
                return PlayerPrefs.GetString(LastUsedBody);
            
            default:
                throw new ArgumentOutOfRangeException(nameof(skinType), skinType, null);
        }
    }
}
