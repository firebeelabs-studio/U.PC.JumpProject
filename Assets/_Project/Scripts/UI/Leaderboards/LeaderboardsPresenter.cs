using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class LeaderboardsPresenter : MonoBehaviour
{
    [SerializeField] private List<LeaderboardsPlayerRowTemplate> _scoreRows;
    [SerializeField] private LeaderboardsPlayerRowTemplate _yourScoreRow;
    [SerializeField] private bool _updatePlace;
    [SerializeField] private TMP_Text _yourPositionText;
    private List<LeaderboardEntry> _scores = new();
    private LeaderboardEntry _yourScore;
    
    private void ReloadData()
    {
        for (int i = 0; i < _scoreRows.Count; i++)
        {
            _scoreRows[i].ClearRow(i);
        }
        _yourScoreRow.ClearRow(0);
        if (_scores.Count > 0)
        {
            for (int i = 0; i < _scores.Count; i++)
            {
                if (_scores[i] == null) continue;
                
                _scoreRows[i].DisplayData(_scores[i], true);
            }
        }

        if (_yourScore == null)
        {
            _yourScore = new LeaderboardEntry()
            {
                Rank = 0,
                Score = 0,
                Player = new LootLockerPlayerData()
                {
                    Name = LoginManager.Instance.Nick,
                    Id = LoginManager.Instance.PlayerId
                },
                Metadata = ",,,,"
            };
        }
        if (_yourPositionText != null)
        {
            _yourPositionText.text = _yourScore.Rank.ToString();
        }
        _yourScoreRow.DisplayData(_yourScore, true);
    }

    public void LoadTopScoresByLevelName(string levelName, float newScore = 0, int takePositions = 3, int skipPositions = 0)
    {
        _scores.Clear();
        _scores = LeaderboardsManagerClient.Instance.Scores[levelName].Entries.OrderBy(e => e.Rank).Skip(skipPositions).Take(takePositions).ToList();
        _yourScore = LeaderboardsManagerClient.Instance.Scores[levelName].Entries.FirstOrDefault(e => e.Player.Id == LoginManager.Instance.PlayerId);
        if (newScore > 0)
        {
            var builder = new StringBuilder();
            foreach (var outfitData in SkinsHolder.Instance.Skins)
            {
                if (outfitData == null) continue;

                builder.Append(outfitData.Id);
                if (outfitData != SkinsHolder.Instance.Skins.Last())
                {
                    builder.Append(",");
                }
            }
            //IF FOUND PREVIOUS SCORE
            if (_yourScore != null)
            {
                _scores.Remove(_scores.FirstOrDefault(s => s.Player.Id == _yourScore.Player.Id));
                //IF BEAT A RECORD
                if (newScore < _yourScore.Score)
                {
                    _yourScore.Score = (int)newScore;
                }
            }
            else
            {
                _yourScore = new LeaderboardEntry()
                {
                    Player = new LootLockerPlayerData()
                    {
                        Name = LoginManager.Instance.Nick,
                        Id = LoginManager.Instance.PlayerId
                    },
                    Score = (int)newScore,
                };
            }
            _scores.Add(_yourScore);
            _scores = _scores.OrderBy(s => s.Rank).Skip(skipPositions).Take(takePositions).ToList();
            _yourScore.Rank = _scores.IndexOf(_yourScore) + 1;
            _yourScore.Metadata = builder.ToString();
        }

        ReloadData();
    }
}
