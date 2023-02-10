using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLeaderboardsPresenter : MonoBehaviour
{
    [SerializeField] private LeaderboardsPlayerRowTemplate _firstPlace;
    [SerializeField] private LeaderboardsPlayerRowTemplate _secondPlace;
    [SerializeField] private LeaderboardsPlayerRowTemplate _thirdPlace;
    [SerializeField] private LeaderboardsPlayerRowTemplate _yourPlace;

    public void ReloadData()
    {
        //_firstPlace.DisplayData();
        //_secondPlace.DisplayData();
        //_thirdPlace.DisplayData();
        //_yourPlace.DisplayData();
    }
}
