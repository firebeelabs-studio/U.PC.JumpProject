using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishSinglePlayer : MonoBehaviour
{
    public static event Action<float> RunFinish;
    public static bool IsFinished;

    // sounds
    [SerializeField] private AudioClip _finishSound;
    private AudioPlayer _audioPlayer;
    private float _score;

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (IsFinished) return;
            IsFinished = true;
            SendNewScore();
            RunFinish?.Invoke(_score);
            _audioPlayer.PlayOneShotSound(_finishSound);
            StartRun.RunStarted = false;
        }
    }

    public static void InvokeRunFinish()
    {
        IsFinished = true;
        RunFinish?.Invoke(0);
        StartRun.RunStarted = false;
    }

    private void SendNewScore()
    {
        _score = FindObjectOfType<TimerSinglePlayer>().TimeInSeconds;
        string levelName = SceneManager.GetActiveScene().name;
        int playerId = LoginManager.Instance.PlayerId;
        var playerEntry = LeaderboardsManagerClient.Instance.Scores[levelName].Entries
            .FirstOrDefault(entry => entry.Player.Id == playerId);
        if (playerEntry != null)
        {
            float bestScore = playerEntry.Score;
            
            if (_score >= bestScore) return;
        }
        
        string bodyId = SkinsHolder.Instance.Skins.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Body)?.Id;
        string hatId = SkinsHolder.Instance.Skins.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Hat)?.Id;
        string eyesId = SkinsHolder.Instance.Skins.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Eyes)?.Id;
        string mouthId = SkinsHolder.Instance.Skins.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Mouth)?.Id;
        string jacketId = SkinsHolder.Instance.Skins.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Jacket)?.Id;
        string skinsIds = $"{bodyId},{hatId},{eyesId},{mouthId},{jacketId}";
        LeaderboardsManagerClient.Instance.SendNewScoreToServer(_score, skinsIds, levelName);
    }
}
