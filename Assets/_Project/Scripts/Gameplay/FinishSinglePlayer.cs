using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishSinglePlayer : MonoBehaviour
{
    public static event Action RunFinish;
    public static bool IsFinished;

    // sounds
    [SerializeField] private AudioClip _finishSound;
    private AudioPlayer _audioPlayer;

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
            RunFinish?.Invoke();
            _audioPlayer.PlayOneShotSound(_finishSound);
            StartRun.RunStarted = false;
            SendNewScore();
        }
    }

    public static void InvokeRunFinish()
    {
        IsFinished = true;
        RunFinish?.Invoke();
        StartRun.RunStarted = false;
    }

    private void SendNewScore()
    {
        float score = FindObjectOfType<TimerSinglePlayer>().TimeInSeconds;
        string levelName = SceneManager.GetActiveScene().name;
        int playerId = LoginManager.Instance.PlayerId;
        var playerEntry = LeaderboardsManagerClient.Instance.Scores[levelName].Entries
            .FirstOrDefault(entry => entry.Player.Id == playerId);
        if (playerEntry != null)
        {
            float bestScore = playerEntry.Score;
            
            if (score >= bestScore) return;
        }

        string bodyId = SkinsHolder.Instance.Skins.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Body)?.Id;
        string hatId = SkinsHolder.Instance.Skins.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Hat)?.Id;
        string eyesId = SkinsHolder.Instance.Skins.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Eyes)?.Id;
        string mouthId = SkinsHolder.Instance.Skins.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Mouth)?.Id;
        string jacketId = SkinsHolder.Instance.Skins.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Jacket)?.Id;
        SkinsIds skinsIds = new SkinsIds(bodyId, hatId, eyesId, mouthId, jacketId);
        string skinsIdsSerialized = JsonConvert.SerializeObject(skinsIds, Formatting.Indented);
        LeaderboardsManagerClient.Instance.SendNewScoreToServer(score, skinsIdsSerialized, levelName);
    }
}
