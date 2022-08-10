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
        //xxxxx
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeIndex(1);
            ChangeCharacter(_characterIndex);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeIndex(-1);
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
    //works only for 1 or -1 TODO: Move this to Utilities, make it more complex (make it works for bigger numbers)
    private void ChangeIndex(int number)
    {
        if (number > 0)
        {
            if (_characterIndex + number > _spawnedCharacters.Count - 1)
            {
                _characterIndex = 0;
            }
            else
            {
                _characterIndex += number;
            }
        }
        else if (number < 0)
        {
            if (_characterIndex + number < 0)
            {
                _characterIndex = _spawnedCharacters.Count - 1;
            }
            else
            {
                _characterIndex += number;
            }
        }
    }
}
