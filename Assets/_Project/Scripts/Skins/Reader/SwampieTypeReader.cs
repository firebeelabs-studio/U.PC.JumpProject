using System;
using System.Collections.Generic;
using UnityEngine;

public class SwampieTypeReader : MonoBehaviour
{
    [SerializeField] private GameObject _blueSwampie;
    [SerializeField] private GameObject _yellowSwampie;
    [SerializeField] private GameObject _greenSwampie;
    [SerializeField] private GameObject _turquoiseSwampie;
    [SerializeField] private GameObject _purpleSwampie;
    public static SwampieSkin.SwampieType SwampieType { get; private set; }
    public static event Action<List<OutfitData>> SwampieInstantiated;

    private void Awake()
    {
        if (!SkinsHolder.Instance || SkinsHolder.Instance.Skins.Count == 0)
        {
            InstantiateSwampie(_blueSwampie);
            SwampieType = SwampieSkin.SwampieType.Blue;
            return;
        }
        
        ChangeSprite(SkinsHolder.Instance.Skins);
    }

    private void ChangeSprite(List<OutfitData> instanceSkins)
    {

        if (instanceSkins.Count > 0)
        {
            SwampieType = instanceSkins[0].swampieType;
        }
        
        switch (SwampieType)
        {
            case SwampieSkin.SwampieType.Blue:
                InstantiateSwampie(_blueSwampie);
                break;
            case SwampieSkin.SwampieType.Green:
                InstantiateSwampie(_greenSwampie);
                break;
            case SwampieSkin.SwampieType.Yellow:
                InstantiateSwampie(_yellowSwampie);
                break;
            case SwampieSkin.SwampieType.Turquoise:
                InstantiateSwampie(_turquoiseSwampie);
                break;
            case SwampieSkin.SwampieType.Purple:
                InstantiateSwampie(_purpleSwampie);
                break;
        }
    }

    private void InstantiateSwampie(GameObject swampieToSpawn)
    {
        var obj = Instantiate(swampieToSpawn, transform);
        obj.transform.position = transform.position;
        if (!SkinsHolder.Instance) return;
        
        SwampieInstantiated?.Invoke(SkinsHolder.Instance.Skins);
    }
}
