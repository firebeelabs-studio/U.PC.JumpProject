using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private PetMenuInteraction _petMenu;
    // Start is called before the first frame update
    private void Start()
    {
        List<OutfitData> skinList = SkinsHolder.Instance.Skins.Count != 0 ? SkinsHolder.Instance.Skins : SkinsHolder.Instance.LastUsedSkins;
        
        if (skinList.Count == 0) return;
        
        SwampieSkin.SwampieType swampieType = skinList[0].swampieType;
        switch (swampieType)
        {
            case SwampieSkin.SwampieType.Blue:
                DisablePawns();
                _blueSwampie.SetActive(true);
                break;
            case SwampieSkin.SwampieType.Green:
                DisablePawns();
                _greenSwampie.SetActive(true);
                break;
            case SwampieSkin.SwampieType.Turquoise:
                DisablePawns();
                _turquoiseSwampie.SetActive(true);
                break;
            case SwampieSkin.SwampieType.Yellow:
                DisablePawns();
                _yellowSwampie.SetActive(true);
                break;
            case SwampieSkin.SwampieType.Purple:
                DisablePawns();
                _purpleSwampie.SetActive(true);
                break;
        }
        _petMenu.CurrentPawn = _currentPawn;
        _menuMan._pawn = _currentPawn;
    }

    private void DisablePawns()
    {
        _blueSwampie.SetActive(false);
        _greenSwampie.SetActive(false);
        _turquoiseSwampie.SetActive(false);
        _yellowSwampie.SetActive(false);
        _purpleSwampie.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
