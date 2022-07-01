using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SearchView : View
{
    [SerializeField] private TMP_Text _textSearch;
    private string text = "...";

    IEnumerator Start()
    {
        var waitTimer = new WaitForSeconds(.25f);
        while (true)
        {
            _textSearch.text = "Waiting for players";
            foreach (char c in text)
            {
                _textSearch.text = _textSearch.text + c;
                yield return waitTimer;
            }
        }
    }

    private void Update()
    {
        //_textSearch.text = "Waiting for players 2/5";
    }
}
