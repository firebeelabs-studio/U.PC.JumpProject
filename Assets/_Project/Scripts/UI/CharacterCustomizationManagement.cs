using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomizationManagement : MonoBehaviour
{
    private List<SwampieSkin> _sortedSkins = new();
    private List<List<SwampieSkin>> _allSortedSkins = new();
    
    private int _numberOfPages;
    [SerializeField] private TMP_Text _textPageNumber;
    [SerializeField] private Button _buttonAll;
    [SerializeField] private Button _buttonHats;
    [SerializeField] private Button _buttonColor;
    [SerializeField] private Button _buttonBody;
    [SerializeField] private Button _buttonEyes;
    [SerializeField] private Button _buttonMouth;
    [SerializeField] private Button _buttonClothes;
    [SerializeField] private Button _buttonAccessories;
    [SerializeField] private Button _buttonArrowRight;
    [SerializeField] private Button _buttonArrowLeft;
    [SerializeField] private Button _buttonBack;

    [SerializeField] private SkinReader _hat;
    [SerializeField] private SkinReader _jacket;
    [SerializeField] private SkinReader _mouth;
    [SerializeField] private SkinReader _color;
    [SerializeField] private SkinReader _eyes;
    [SerializeField] private SwampieTypeChanger _body;
    private ClearSkins _clearSkins;

    [SerializeField] private int _pageSize = 10;
    private int _lastPage;
    private int _currentPage;

    [SerializeField] private GameObject _gridHolder;
    [SerializeField] private GameObject _template;
    [SerializeField] private GameObject _templateOff;
    private List<GameObject> _gridCells = new();
    private bool _allSkinsLoaded;
    private bool _canInitializeFirsTime = true;
    [SerializeField] private SwampieTypeChanger _typeChanger;
    [SerializeField] private List<Sprite> _typeSprites;

    private void Awake()
    {
        _clearSkins = _buttonBack.GetComponent<ClearSkins>();
    }

    private void Start()
    {
        _buttonBack.onClick.AddListener(() =>
        {
            _hat.SaveSkin();
            _jacket.SaveSkin();
            _mouth.SaveSkin();
            _color.SaveSkin();
            _eyes.SaveSkin();
            _clearSkins.Clear();
            PlayerPrefsSaveAndLoad.SaveLastUsedSkin(SwampieSkin.SkinType.Hat, SkinsHolder.Instance.Skins.FirstOrDefault(t => t.skinType == SwampieSkin.SkinType.Hat)?.Id);
            PlayerPrefsSaveAndLoad.SaveLastUsedSkin(SwampieSkin.SkinType.Eyes, SkinsHolder.Instance.Skins.FirstOrDefault(t => t.skinType == SwampieSkin.SkinType.Eyes)?.Id);
            PlayerPrefsSaveAndLoad.SaveLastUsedSkin(SwampieSkin.SkinType.Mouth, SkinsHolder.Instance.Skins.FirstOrDefault(t => t.skinType == SwampieSkin.SkinType.Mouth)?.Id);
            PlayerPrefsSaveAndLoad.SaveLastUsedSkin(SwampieSkin.SkinType.Jacket, SkinsHolder.Instance.Skins.FirstOrDefault(t => t.skinType == SwampieSkin.SkinType.Jacket)?.Id);
            PlayerPrefsSaveAndLoad.SaveLastUsedSkin(SwampieSkin.SkinType.Body, SkinsHolder.Instance.Skins.FirstOrDefault(t => t.skinType == SwampieSkin.SkinType.Body)?.Id);
            LoadingScreenCanvas.Instance?.LoadScene("MainMenu");
        });
        _buttonAll.onClick.AddListener(LoadAllSkins);
        _buttonHats.onClick.AddListener(() => LoadSkinsBySkinType(SwampieSkin.SkinType.Hat));
        _buttonClothes.onClick.AddListener(() => LoadSkinsBySkinType(SwampieSkin.SkinType.Jacket));
        _buttonMouth.onClick.AddListener(() => LoadSkinsBySkinType(SwampieSkin.SkinType.Mouth));
        _buttonColor.onClick.AddListener(() => LoadSkinsBySkinType(SwampieSkin.SkinType.Body));
        _buttonEyes.onClick.AddListener(() => LoadSkinsBySkinType(SwampieSkin.SkinType.Eyes));
        _buttonBody.onClick.AddListener(ShowTypes);
        _buttonArrowRight.onClick.AddListener(() =>
        {
            _buttonArrowLeft.interactable = true;
            ClearGrid();
            if (_currentPage < _lastPage)
            {
                _currentPage++;
                ChangePage(_currentPage);
            }
            
            if(_currentPage >= _lastPage)
            {
                _buttonArrowRight.interactable = false;
            }
        });
        _buttonArrowLeft.onClick.AddListener(() =>
        {
            _buttonArrowRight.interactable = true;
            ClearGrid();
            if (_currentPage > 0)
            {
                _currentPage--;
                ChangePage(_currentPage);
            }

            if (_currentPage <= 0)
            {
                _buttonArrowLeft.interactable = false;
            }
        });
    }

    private void Update()
    {
        if (_canInitializeFirsTime)
        {
            _buttonAll.Select();
            _buttonAll.onClick.Invoke();
            _canInitializeFirsTime = false;
        }
    }

    private void ResetArrows()
    {
        _buttonArrowLeft.interactable = false;
        if (_lastPage == 0)
        {
            _buttonArrowRight.interactable = false;
        }
        else
        {
            _buttonArrowRight.interactable = true;
        }
    }

    private void ChangePage(int newPage)
    {
        if (_allSkinsLoaded)
        {
            InitializeGridWithAllSkins(newPage);
        }
        else
        {
            InitializeGrid2(newPage);
        }
        
        _textPageNumber.SetText($"PAGE {newPage + 1}");
    }
    
    private void InitializeGrid2(int currentPage)
    {
        //create cells with skins
        int startingIndex = 10 * currentPage;
        int createdCells = 0;
        for (int i = startingIndex; i < _pageSize + startingIndex && i < _sortedSkins.Count; i++)
        {
            GameObject newGridCell = Instantiate(_template, _gridHolder.transform);
            SkinGridTemplate references = newGridCell.GetComponent<SkinGridTemplate>();
            references.SkinImage.enabled = true;
            references.SkinImage.sprite = _sortedSkins[i].DisplaySprite;
            references.Id = i;
            references.SkinType = _sortedSkins[i].skinType;
            newGridCell.GetComponent<Button>().onClick.AddListener((() =>
            {
                ChangeSkin(references.SkinType, references.Id);
            }));
            _gridCells.Add(newGridCell);
            createdCells++;
        }
        _allSkinsLoaded = false;
        double maxPage = _sortedSkins.Count / 10.0;
        _lastPage = Convert.ToInt32(Math.Ceiling(maxPage) - 1);
        
        //create missing cells
        for (int i = 0; i < 10 - createdCells; i++)
        {
            GameObject newGridCell = Instantiate(_templateOff, _gridHolder.transform);
            _gridCells.Add(newGridCell);
        }
    }
    private void InitializeGridWithAllSkins(int currentPage)
    {
        _allSortedSkins.Clear();
        _allSortedSkins.Add(_jacket.SortedSkins);
        _allSortedSkins.Add(_hat.SortedSkins);
        _allSortedSkins.Add(_mouth.SortedSkins);
        _allSortedSkins.Add(_color.SortedSkins);
        _allSortedSkins.Add(_eyes.SortedSkins);

        //--------TODO: add remaining elements--------
        
        //create cells with skins
        int startingIndex = 10 * currentPage;

        foreach (var sortedSkins in _allSortedSkins)
        {
            for (int i = 0; i < sortedSkins.Count; i++)
            {
                GameObject newGridCell = Instantiate(_template, _gridHolder.transform);
                SkinGridTemplate references = newGridCell.GetComponent<SkinGridTemplate>();
                references.SkinImage.enabled = true;
                references.SkinImage.sprite = sortedSkins[i].DisplaySprite;
                references.Id = i;
                references.SkinType = sortedSkins[i].skinType;
                newGridCell.GetComponent<Button>().onClick.AddListener((() =>
                {
                    ChangeSkin(references.SkinType, references.Id);
                }));
                _gridCells.Add(newGridCell);
            }
        }

        foreach (var gridCell in _gridCells)
        {
            gridCell.SetActive(false);
        }

        int activeCells = 0;
        for (int i = startingIndex; i < _pageSize + startingIndex && i < _gridCells.Count; i++)
        {
            _gridCells[i].SetActive(true);
            activeCells++;
        }
        
        //Create missing tiles
        if (activeCells < 10)
        {
            for (int i = 0; i < 10 - activeCells; i++)
            {
                GameObject newGridCell = Instantiate(_templateOff, _gridHolder.transform);
                _gridCells.Add(newGridCell);
            }
        }
        

        _allSkinsLoaded = true;
        double maxPage = _gridCells.Count / 10.0;
        _lastPage = Convert.ToInt32(Math.Ceiling(maxPage) - 1);
        //create missing cells
    }
    private void ChangeSkin(SwampieSkin.SkinType skinType, int id)
    {
        switch (skinType)
        {
            case SwampieSkin.SkinType.Hat:
                _hat.ChangeSkin(id);
                break;
            case SwampieSkin.SkinType.Jacket:
                _jacket.ChangeSkin(id);
                break;
            case SwampieSkin.SkinType.Mouth:
                _mouth.ChangeSkin(id);
                break;
            case SwampieSkin.SkinType.Body:
                _color.ChangeSkin(id);
                break;
            case SwampieSkin.SkinType.Eyes:
                _eyes.ChangeSkin(id);
                break;
            
            //--------TODO: add remaining elements--------
            
        }
    }

    private void ClearGrid()
    {
        foreach (var gridCell in _gridCells)
        {
            Destroy(gridCell);
        }
        _gridCells.Clear();
    }

    private void LoadSkinsBySkinType(SwampieSkin.SkinType skinType)
    {
        _currentPage = 0;
        _textPageNumber.text = "PAGE 1";
        ClearGrid();
        switch (skinType)
        {
            case SwampieSkin.SkinType.Hat:
                _sortedSkins = _hat.SortedSkins;
                break;
            case SwampieSkin.SkinType.Jacket:
                _sortedSkins = _jacket.SortedSkins;
                break;
            case SwampieSkin.SkinType.Mouth:
                _sortedSkins = _mouth.SortedSkins;
                break;
            case SwampieSkin.SkinType.Body:
                _sortedSkins = _color.SortedSkins;
                break;
            case SwampieSkin.SkinType.Eyes:
                _sortedSkins = _eyes.SortedSkins;
                break;
        }
        InitializeGrid2(_currentPage);
        ResetArrows();
    }

    private void ShowTypes()
    {
        _currentPage = 0;
        _textPageNumber.text = "PAGE 1";
        ClearGrid();
        InitializeGridWithTypes(_currentPage);
        ResetArrows();
    }

    private void InitializeGridWithTypes(int currentPage)
    {
        //create cells with skins
        int startingIndex = 10 * currentPage;
        int createdCells = 0;
        for (int i = startingIndex; i < _pageSize + startingIndex && i < _typeSprites.Count; i++)
        {
            GameObject newGridCell = Instantiate(_template, _gridHolder.transform);
            SkinGridTemplate references = newGridCell.GetComponent<SkinGridTemplate>();
            references.SkinImage.enabled = true;
            references.SkinImage.sprite = _typeSprites[i];
            references.Id = i;
            newGridCell.GetComponent<Button>().onClick.AddListener((() =>
            {
                ChangeSwampieType(references.Id);
            }));
            _gridCells.Add(newGridCell);
            createdCells++;
        }
        _allSkinsLoaded = false;
        double maxPage = 1;
        _lastPage = Convert.ToInt32(Math.Ceiling(maxPage) - 1);
        
        //create missing cells
        for (int i = 0; i < 10 - createdCells; i++)
        {
            GameObject newGridCell = Instantiate(_templateOff, _gridHolder.transform);
            _gridCells.Add(newGridCell);
        }
    }

    private void ChangeSwampieType(int index)
    {
        _typeChanger.ReplaceSwampie(index);
    }

    private void LoadAllSkins()
    {
        _currentPage = 0;
        ClearGrid();
        _sortedSkins = _hat.LoadAllSkins();
        InitializeGridWithAllSkins(_currentPage);
        ResetArrows();
    }
    
}
