using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChanger : MonoBehaviour
{
    [SerializeField] private List<GameObject> _characters;
    [SerializeField] private List<GameObject> _spawnedCharacters;
    private GameObject _activeCharacter;
    private int _characterIndex = 0;

    private void Awake()
    {
        SpawnVisuals();
        _activeCharacter = _spawnedCharacters[0];
    }
    private void Start()
    {
        ChangeCharacter(_characterIndex);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _characterIndex = ArcnesTools.IndexHelper.LoopIndex(1, _characterIndex, _spawnedCharacters);
            ChangeCharacter(_characterIndex);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _characterIndex = ArcnesTools.IndexHelper.LoopIndex(-1, _characterIndex, _spawnedCharacters);
            ChangeCharacter(_characterIndex);
        }
    }
    private void ChangeCharacter(int index)
    {
        _activeCharacter.SetActive(false);
        GameObject newActiveCharacter = _spawnedCharacters[index];
        newActiveCharacter.SetActive(true);
        _activeCharacter = newActiveCharacter;
    }
    private void SpawnVisuals()
    {
        foreach (GameObject character in _characters)
        {
            GameObject spawnedPlayer = Instantiate(character, transform);
            _spawnedCharacters.Add(spawnedPlayer);
            spawnedPlayer.SetActive(false);
        }
    }
}
