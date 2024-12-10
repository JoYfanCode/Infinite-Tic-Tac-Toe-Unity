using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VInspector;

public class ScenesChanger : MonoBehaviour
{
    [Tab("Buttons")]

    [SerializeField] private Button _startGameAINormalButton;
    [SerializeField] private Button _startGameAIHardButton;
    [SerializeField] private Button _startGameAIVeryHardButton;
    [SerializeField] private Button _startGameTwoPlayersButton;
    [SerializeField] private Button _openMenuButton;

    [Tab("Objects")]

    [SerializeField] private SceneChangerAnimation _sceneChangerAnimation;

    private const string MENU_SCENE = "Menu";
    private const string GAME_FIELD = "Game Field";

    private void OnEnable()
    {
        _startGameAINormalButton?.onClick.AddListener(StartGameAINormal);
        _startGameAIHardButton?.onClick.AddListener(StartGameAIHard);
        _startGameAIVeryHardButton?.onClick.AddListener(StartGameAIVeryHard);

        _startGameTwoPlayersButton?.onClick.AddListener(StartGameTwoPlayers);

        _openMenuButton?.onClick.AddListener(StartMenu);
    }

    private void OnDisable()
    {
        _startGameAINormalButton?.onClick.RemoveListener(StartGameAINormal);
        _startGameAIHardButton?.onClick.RemoveListener(StartGameAIHard);
        _startGameAIVeryHardButton?.onClick.RemoveListener(StartGameAIVeryHard);

        _startGameTwoPlayersButton?.onClick.RemoveListener(StartGameTwoPlayers);

        _openMenuButton?.onClick.RemoveListener(StartMenu);
    }

    private void OpenSceneMenu()
    {
        _sceneChangerAnimation.OnFinishedAppear -= OpenSceneMenu;
        SceneManager.LoadScene(MENU_SCENE, LoadSceneMode.Single);
    }

    private void OpenSceneGameField() 
    {
        _sceneChangerAnimation.OnFinishedAppear -= OpenSceneGameField;
        SceneManager.LoadScene(GAME_FIELD, LoadSceneMode.Single);
    }

    private void StartMenu()
    {
        AudioSystem.PlaySound(AudioSystem.inst.Click);
        SetUp.Mode = Modes.TwoPlayers;
        _sceneChangerAnimation.Appear();
        _sceneChangerAnimation.OnFinishedAppear += OpenSceneMenu;
    }

    private void StartGameTwoPlayers()
    {
        AudioSystem.PlaySound(AudioSystem.inst.Click);
        SetUp.Mode = Modes.TwoPlayers;
        _sceneChangerAnimation.Appear();
        _sceneChangerAnimation.OnFinishedAppear += OpenSceneGameField;
    }

    private void StartGameAINormal()
    {
        AudioSystem.PlaySound(AudioSystem.inst.Click);
        SetUp.Mode = Modes.AINormal;
        _sceneChangerAnimation.Appear();
        _sceneChangerAnimation.OnFinishedAppear += OpenSceneGameField;
    }

    private void StartGameAIHard()
    {
        AudioSystem.PlaySound(AudioSystem.inst.Click);
        SetUp.Mode = Modes.AIHard;
        _sceneChangerAnimation.Appear();
        _sceneChangerAnimation.OnFinishedAppear += OpenSceneGameField;
    }

    private void StartGameAIVeryHard()
    {
        AudioSystem.PlaySound(AudioSystem.inst.Click);
        SetUp.Mode = Modes.AIVeryHard;
        _sceneChangerAnimation.Appear();
        _sceneChangerAnimation.OnFinishedAppear += OpenSceneGameField;
    }
}
