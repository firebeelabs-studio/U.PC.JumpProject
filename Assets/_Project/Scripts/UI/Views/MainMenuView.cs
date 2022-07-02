using UnityEngine;
using UnityEngine.UI;
public class MainMenuView : View
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _levelCreatorButton;
    [SerializeField] private Button _profileButton;
    [SerializeField] private SceneLoader _sceneLoader;
    

    //dont Call this on client or everything will brake
    public override void Initialize()
    {
        _startButton.onClick.AddListener(() =>
        {
            _sceneLoader.LoadScene();
        });
        base.Initialize();
    }
}
