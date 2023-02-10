using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsManagement : MonoBehaviour
{
    [Header("TEMPLATE")]
    [SerializeField] private GameObject _template;
    [Space(10)]
    [SerializeField] private Transform _contentHolder;
    [SerializeField] private Button _closeButton;


    //TEMP
    private List<(string Nickname, float Time)> _tempScores = new();
    //
    
    private List<GameObject> _playerScoresObj;
    private void Start()
    {
        _closeButton.onClick.AddListener(() =>
        {
            gameObject.transform.DOScale(Vector2.zero, 0.25f).SetEase(Ease.InOutCubic).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });

    }
    private void OnEnable()
    {
        if(_playerScoresObj is null) 
            _playerScoresObj = new();

        //TODO: get list of scores
        //List<T> receivedData = new();
        //List<T> receivedData = ListFromOutside().ToList();
        //TODO: sort data
        //receivedData.OrderBy(t => t.Time);
        //TODO: instantiate objects from sorted list
        //int place = 1;
        //foreach (playerScore in receivedData)
        //{
        //  InstantiatePlayerScore(place, playerScore.Nickname, playerScore.Time);
        //  place++;
        //}


        //TEMP
        _tempScores.Add(new("GiorgioGiovanni", 21.37f));
        _tempScores.Add(new("xXxGigaKox2007PLxXx", 20.07f));
        _tempScores.Add(new("Maciek2000", 2000.00f));
        _tempScores.Add(new("Endrju", 65.35f));
        _tempScores.Add(new("Mariusz Pudzianowski", 66.76f));
        _tempScores.Add(new("Piotr Luszcz", 26.12f));
        _tempScores.Add(new("Paluch", 129.52f));
        var tempOrdered = _tempScores.OrderBy(s => s.Time).ToList();
        int place = 1;
        foreach (var item in tempOrdered)
        {
            InstantiatePlayerScore(place, item.Nickname, item.Time);
            place++;
        }
        //


        gameObject.transform.localScale = Vector2.zero;
        gameObject.transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.InOutCubic);
    }
    private void OnDisable()
    {
        foreach (GameObject playerScoreObj in _playerScoresObj)
        {
            Destroy(playerScoreObj);
        }
        _playerScoresObj.Clear();

        //TEMP
        _tempScores.Clear();
        //
    }
    private void InstantiatePlayerScore(int place, string nickname, float time)
    {
        GameObject newPlayerScore = Instantiate(_template, _contentHolder);
        PlayerScoreTemplate references = newPlayerScore.GetComponent<PlayerScoreTemplate>();
        newPlayerScore.name = nickname;
        references.Place.SetText($"{place}. ");
        references.Nickname.SetText(nickname.ToString());
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        
        references.Time.SetText(minutes.ToString("00") + ":" + seconds.ToString("00"));

        _playerScoresObj.Add(newPlayerScore);

    }
}
