using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReadPetType : MonoBehaviour
{
    [SerializeField] private GameObject _blueSwampie;
    [SerializeField] private GameObject _greenSwampie;
    [SerializeField] private GameObject _turquoiseSwampie;
    [SerializeField] private GameObject _yellowSwampie;
    [SerializeField] private GameObject _purpleSwampie;
    private GameObject _currentPawn;

    [SerializeField] private MenuManagement _menuMan;
    // Start is called before the first frame update
    private void Start()
    {
        List<OutfitData> skinList = SkinsHolder.Instance.Skins.Count != 0 ? SkinsHolder.Instance.Skins : SkinsHolder.Instance.LastUsedSkins;

        if (skinList.Count == 0)
        {
            List<string> ids = new()
            {
                PlayerPrefsSaveAndLoad.LoadLastUsedSkin(SwampieSkin.SkinType.Hat),
                PlayerPrefsSaveAndLoad.LoadLastUsedSkin(SwampieSkin.SkinType.Eyes),
                PlayerPrefsSaveAndLoad.LoadLastUsedSkin(SwampieSkin.SkinType.Mouth),
                PlayerPrefsSaveAndLoad.LoadLastUsedSkin(SwampieSkin.SkinType.Jacket),
                PlayerPrefsSaveAndLoad.LoadLastUsedSkin(SwampieSkin.SkinType.Body)
            };

            if (SkinsHolder.Instance.AllSkinsSO.Where(skin => ids.Contains(skin.Id)).ToList().Count == 0) return;

            foreach (var swampieSkin in SkinsHolder.Instance.AllSkinsSO.Where(skin => ids.Contains(skin.Id)))
            {
                SkinsHolder.Instance.LastUsedSkins.Add(new OutfitData
                {
                    Id = swampieSkin.Id,
                    SkinSprite = swampieSkin.SkinSprite,
                    Position = swampieSkin.Positions[0],
                    skinType = swampieSkin.skinType,
                    swampieType = swampieSkin.swampieType
                });
            }
        }
        
        SwampieSkin.SwampieType swampieType = skinList[0].swampieType;
        switch (swampieType)
        {
            case SwampieSkin.SwampieType.Blue:
                DisablePawns();
                _blueSwampie.SetActive(true);
                _currentPawn = _blueSwampie;
                break;
            case SwampieSkin.SwampieType.Green:
                DisablePawns();
                _greenSwampie.SetActive(true);
                _currentPawn = _greenSwampie;
                break;
            case SwampieSkin.SwampieType.Turquoise:
                DisablePawns();
                _turquoiseSwampie.SetActive(true);
                _currentPawn = _turquoiseSwampie;
                break;
            case SwampieSkin.SwampieType.Yellow:
                DisablePawns();
                _yellowSwampie.SetActive(true);
                _currentPawn = _yellowSwampie;
                break;
            case SwampieSkin.SwampieType.Purple:
                DisablePawns();
                _purpleSwampie.SetActive(true);
                _currentPawn = _purpleSwampie;
                break;
        }
        
        _menuMan._pawn = _currentPawn;
        foreach (var outfitReader in _currentPawn.GetComponentsInChildren<OutfitReader>())
        {
            outfitReader.LoadSkins();
        }
    }

    private void DisablePawns()
    {
        _blueSwampie.SetActive(false);
        _greenSwampie.SetActive(false);
        _turquoiseSwampie.SetActive(false);
        _yellowSwampie.SetActive(false);
        _purpleSwampie.SetActive(false);
    }
}
