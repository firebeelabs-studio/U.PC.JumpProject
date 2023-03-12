using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LeaderboardsPresenter : MonoBehaviour
{
    [SerializeField] private List<LeaderboardsPlayerRowTemplate> _scoreRows;
    [SerializeField] private LeaderboardsPlayerRowTemplate _yourScoreRow;
    [SerializeField] private bool _updatePlace;
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
                
                _scoreRows[i].DisplayData(_scores[i], _updatePlace);
            }
        }

        if (_yourScore == null)
        {
            _yourScoreRow.DisplayData(new LeaderboardEntry()
            {
                Rank = 0,
                Score = 0,
                Player = new LootLockerPlayerData()
                {
                    Name = LoginManager.Instance.Nick
                }
            }, true);
        }
        else
        {
            _yourScoreRow.DisplayData(_yourScore, true);
        }
    }

    public void LoadTopScoresByLevelName(string levelName, float newScore = 0, int count = 3, int startingPosition = 0)
    {
        _scores.Clear();
        if (count != 0)
        {
            _scores = LeaderboardsManagerClient.Instance.Scores[levelName].Entries.OrderBy(e => e.Score).Skip(startingPosition).ToList();
            _scores = LeaderboardsManagerClient.Instance.Scores[levelName].Entries.OrderBy(e => e.Score).Take(count).ToList();
        }
        else
        {
            _scores = LeaderboardsManagerClient.Instance.Scores[levelName].Entries.OrderBy(e => e.Score).Take(3).ToList();
        }
        _yourScore = LeaderboardsManagerClient.Instance.Scores[levelName].Entries
            .FirstOrDefault(e => e.Player.Id == LoginManager.Instance.PlayerId);
        if (newScore > 0)
        {
            if (_yourScore != null)
            {
                if (newScore < _yourScore.Score)
                {
                    _yourScore.Score = (int)newScore;
                }
            }
            else
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
                _yourScore = new LeaderboardEntry()
                {
                    Player = new LootLockerPlayerData()
                    {
                        Name = LoginManager.Instance.Nick
                    },
                    Score = (int)newScore,
                    Metadata = builder.ToString()
                };
            }
            _scores.Add(_yourScore);
            _scores = _scores.OrderBy(s => s.Score).Take(3).ToList();
            _yourScore.Rank = _scores.IndexOf(_yourScore) + 1;
        }
        
        ReloadData();
    }
}
