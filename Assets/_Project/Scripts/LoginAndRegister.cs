using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginAndRegister : MonoBehaviour
{
    [SerializeField] private LoginManager _loginManager;
    [Header("LOGIN PANEL")] 
    [SerializeField] private GameObject _loginPanel;
    [SerializeField] private TMP_InputField _emailInputFieldLogin;
    [SerializeField] private TMP_InputField _passwordInputFieldLogin;
    [SerializeField] private Button _submitButtonLogin;
    [SerializeField] private Button _switchToRegister;
    [SerializeField] private Toggle _rememberMeToggle;
    private List<TMP_InputField> _loginInputFields;
    
    [Header("REGISTRATION PANEL")]
    [SerializeField] private GameObject _registrationPanel;
    [SerializeField] private TMP_InputField _nicknameInputFieldRegistration;
    [SerializeField] private TMP_InputField _emailInputFieldRegistration;
    [SerializeField] private TMP_InputField _passwordFieldRegistration;
    [SerializeField] private Button _submitButtonRegistration;
    [SerializeField] private Button _switchToLogin;
    private List<TMP_InputField> _registrationInputFields;

    [Header("LOADING")]
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private TMP_Text _loadingText;
    
    private GameObject _currentPanel;
    private int _fieldsIndex;
    
    private void Start()
    {
        _loginInputFields = new List<TMP_InputField>() { _emailInputFieldLogin, _passwordInputFieldLogin };
        _registrationInputFields = new List<TMP_InputField>() { _nicknameInputFieldRegistration, _emailInputFieldRegistration, _passwordFieldRegistration };
        _currentPanel = _loginPanel;
        _submitButtonLogin.onClick.AddListener(Login);
        _submitButtonRegistration.onClick.AddListener(Register);
        _emailInputFieldLogin.onValueChanged.AddListener(value => VerifyInputs(_submitButtonLogin,_emailInputFieldLogin, _passwordInputFieldLogin));
        _emailInputFieldLogin.onSelect.AddListener(value => _fieldsIndex = 0);
        _passwordInputFieldLogin.onValueChanged.AddListener(value => VerifyInputs(_submitButtonLogin,_emailInputFieldLogin, _passwordInputFieldLogin));
        _passwordInputFieldLogin.onSelect.AddListener(value => _fieldsIndex = 1);
        _nicknameInputFieldRegistration.onValueChanged.AddListener(value => VerifyInputs(_submitButtonRegistration, _nicknameInputFieldRegistration, _passwordFieldRegistration));
        _nicknameInputFieldRegistration.onSelect.AddListener(value => _fieldsIndex = 0);
        _passwordFieldRegistration.onValueChanged.AddListener(value => VerifyInputs(_submitButtonRegistration, _nicknameInputFieldRegistration, _passwordFieldRegistration));
        _passwordFieldRegistration.onSelect.AddListener(value => _fieldsIndex = 1);
        _switchToRegister.onClick.AddListener(() =>
        {
            SwitchPanels(_registrationPanel);
            _nicknameInputFieldRegistration.Select();
            _fieldsIndex = 0;
        });
        _switchToLogin.onClick.AddListener(() =>
        {
            SwitchPanels(_loginPanel);
            _emailInputFieldLogin.Select();
            _fieldsIndex = 0;
        });
        _emailInputFieldLogin.Select();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchInputFields(_currentPanel);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchPanels(_loginPanel);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_currentPanel == _loginPanel && _submitButtonLogin.interactable)
            {
                Login();
            }
            else if (_currentPanel == _registrationPanel && _submitButtonRegistration.interactable)
            {
                Register();
            }
            else
            {
                //TODO: SHOW INFO
            }
        }
    }

    private void OnEnable()
    {
        _loadingPanel.SetActive(false);
    }


    private void SwitchPanels(GameObject panelToEnable)
    {
        if (!panelToEnable) return;

        panelToEnable.SetActive(true);
        _currentPanel.SetActive(false);
        _currentPanel = panelToEnable;
    }

    private void SwitchInputFields(GameObject panelWithInputFields)
    {
        if (!panelWithInputFields) return;

        if (panelWithInputFields == _loginPanel)
        {
            _fieldsIndex = ArcnesTools.IndexHelper.LoopIndex(1, _fieldsIndex, _loginInputFields);
            _loginInputFields[_fieldsIndex].Select();
        }
        else if (panelWithInputFields == _registrationPanel)
        {
            _fieldsIndex = ArcnesTools.IndexHelper.LoopIndex(1, _fieldsIndex, _registrationInputFields);
            _registrationInputFields[_fieldsIndex].Select();
        }
    }
    private void VerifyInputs(Button buttonToInteract, TMP_InputField nameField, TMP_InputField passwordField)
    {
        buttonToInteract.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8) 
                                        && (!string.IsNullOrWhiteSpace(nameField.text) && !string.IsNullOrWhiteSpace(passwordField.text));
    }

    private IEnumerator Loading()
    {
        _loadingPanel.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_loadingText.rectTransform.DOScale(1.25f, 1f));
        sequence.Append(_loadingText.rectTransform.DOScale(1f, 1f));
        sequence.SetLoops(-1);
        sequence.Play();
        yield return new WaitWhile(() => LoadingScreenCanvas.Instance.IsNewSceneLoading);
        yield return new WaitForSeconds(2f);
        _loadingPanel.SetActive(false);
    }

    private void Login()
    {
        StartCoroutine(Loading());
        _loginManager.Login(_emailInputFieldLogin.text, _passwordInputFieldLogin.text, _rememberMeToggle.isOn);
    }

    private void Register()
    {
        StartCoroutine(Loading());
        _loginManager.Register(_emailInputFieldRegistration.text, _passwordFieldRegistration.text, _nicknameInputFieldRegistration.text);
    }
}
