using System.Linq;
using UnityEngine;

public class ReplayGhost : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private SpriteRenderer _hat;
    [SerializeField] private SpriteRenderer _eyes;
    [SerializeField] private SpriteRenderer _mouth;
    [SerializeField] private SpriteRenderer _jacket;
    public void InitializeVisuals(ReplayData data)
    {
        if (!string.IsNullOrEmpty(data.BodyId))
        {
            _body.sprite = SkinsHolder.Instance.Skins.FirstOrDefault(outfitData =>
                outfitData.skinType == SwampieSkin.SkinType.Body && outfitData.Id == data.BodyId)?.SkinSprite;
            _hat.sprite = SkinsHolder.Instance.Skins.FirstOrDefault(outfitData =>
                outfitData.skinType == SwampieSkin.SkinType.Hat && outfitData.Id == data.HatId)?.SkinSprite;
            _eyes.sprite = SkinsHolder.Instance.Skins.FirstOrDefault(outfitData =>
                outfitData.skinType == SwampieSkin.SkinType.Eyes && outfitData.Id == data.EyesId)?.SkinSprite;
            _mouth.sprite = SkinsHolder.Instance.Skins.FirstOrDefault(outfitData =>
                outfitData.skinType == SwampieSkin.SkinType.Mouth && outfitData.Id == data.MouthId)?.SkinSprite;
            _jacket.sprite = SkinsHolder.Instance.Skins.FirstOrDefault(outfitData =>
                outfitData.skinType == SwampieSkin.SkinType.Jacket && outfitData.Id == data.JacketId)?.SkinSprite;
        }
    }
}
