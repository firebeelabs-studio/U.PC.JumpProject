using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TopLeaderboardsPresenter : MonoBehaviour
{
    [SerializeField] private List<LeaderboardsPlayerRowTemplate> _scoreRows;
    [SerializeField] private LeaderboardsPlayerRowTemplate _yourScoreRow;
    public List<LeaderboardEntry> _topScores = new();
    public LeaderboardEntry _yourScore;
    
    private void ReloadData()
    {
        for (int i = 0; i < _scoreRows.Count; i++)
        {
            _scoreRows[i].ClearRow(i);
        }
        if (_topScores.Count > 0)
        {
            for (int i = 0; i < _topScores.Count; i++)
            {
                if (_topScores[i] == null) continue;
                
                _scoreRows[i].DisplayData(_topScores[i]);
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
            });
        }
        else
        {
            _yourScoreRow.DisplayData(_yourScore);
        }
    }

    public void LoadTopScoresByLevelName(string levelName)
    {
        _topScores.Clear();
        _topScores = LeaderboardsManagerClient.Instance.Scores[levelName].Entries.OrderBy(e => e.Score).Take(3).ToList();
        _yourScore = LeaderboardsManagerClient.Instance.Scores[levelName].Entries
            .FirstOrDefault(e => e.Player.Id == LoginManager.Instance.PlayerId);
        ReloadData();
    }
}
