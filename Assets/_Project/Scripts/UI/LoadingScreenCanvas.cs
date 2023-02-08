using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScreenCanvas : MonoBehaviour
{
    public static LoadingScreenCanvas Instance { get; private set; }

    //If you are using dotween inside coroutine use this bool to check if new scene isn't loading
    public bool IsNewSceneLoading { get; private set; }
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Image _loadingImageProgress;
    [SerializeField] private Image _crossFadeImg;
    [SerializeField] private float _loadingTime = 2f;
    private Vector2 _completedImgSize;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _completedImgSize = _crossFadeImg.sprite.bounds.size;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        IsNewSceneLoading = true;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        _loadingScreen.SetActive(true);
        _loadingImageProgress.fillAmount = 0;
        _loadingImageProgress.DOFillAmount(1, _loadingTime);

        while (!operation.isDone)
        { 
            if (_loadingImageProgress.fillAmount == 1)
            {
                DOTween.KillAll();
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        if (operation.isDone)
        {
            //_crossFadeImg.DOFillAmount(1, 0.25f).OnComplete(() =>
            //{
            //    _crossFadeImg.fillOrigin = 1;
            //    _crossFadeImg.DOFillAmount(0, 0.25f);
            //    _loadingScreen.SetActive(false);
            //});
            IsNewSceneLoading = false;
            _loadingScreen.SetActive(false);
            _crossFadeImg.gameObject.SetActive(true);
            
            System.Random rnd = new();
            int newRnd = rnd.Next(1,3);
            if (newRnd % 2 == 0)
            {
                float verticalMove = _crossFadeImg.rectTransform.rect.height;
                //vertical
                newRnd = rnd.Next(1, 3);
                _crossFadeImg.rectTransform.DOLocalMoveY(newRnd % 2 == 0 ? verticalMove : -verticalMove, 0.5f).OnComplete(() =>
                {
                    _crossFadeImg.gameObject.SetActive(false);
                    _crossFadeImg.rectTransform.localPosition = Vector3.zero;
                });
                    
            }
            else
            {
                float horizontalMove = _crossFadeImg.rectTransform.rect.width;
                //vertical
                newRnd = rnd.Next(1, 3);
                _crossFadeImg.rectTransform.DOLocalMoveX(newRnd % 2 == 0 ? horizontalMove : -horizontalMove, 0.5f).OnComplete(() =>
                {
                    _crossFadeImg.gameObject.SetActive(false);
                    _crossFadeImg.rectTransform.localPosition = Vector3.zero;
                });
            }
        }
    }
}
