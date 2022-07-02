using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SearchView : View
{
    [SerializeField] private TMP_Text _textSearch;
    public int PlayersCount = 0;
    
    private void Update()
    {
        _textSearch.text = $"Waiting for players {PlayersCount}/5";
    }
}
