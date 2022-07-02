using TMPro;
using UnityEngine;

public class SearchView : View
{
    [SerializeField] private TMP_Text _textSearch;
    private int _playersCount = 0;
    public int PlayersCount
    {
        get { return _playersCount; }
        set
        {
            _playersCount = value;
            ChangeText(value);
        }
    }
    private void Start()
    {
        ChangeText(PlayersCount);
    }
    private void ChangeText(int playersCount)
    {
        _textSearch.text = $"Waiting for players {playersCount}/5";
    }
}
