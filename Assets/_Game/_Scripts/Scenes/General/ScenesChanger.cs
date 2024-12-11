using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VInspector;

public class ScenesChanger : MonoBehaviour
{
    [Tab("Buttons")]

    [SerializeField] private Button startGameAINormalButton;
    [SerializeField] private Button startGameAIHardButton;
    [SerializeField] private Button startGameAIVeryHardButton;
    [SerializeField] private Button startGameTwoPlayersButton;
    [SerializeField] private Button openMenuButton;

    [Tab("Objects")]

    [SerializeField] private SceneChangerAnimation sceneChangerAnimation;

    private const string MENU_SCENE = "Menu";
    private const string GAME_FIELD = "Game Field";

    private void OnEnable()
    {
        startGameAINormalButton?.onClick.AddListener(StartGameAINormal);
        startGameAIHardButton?.onClick.AddListener(StartGameAIHard);
        startGameAIVeryHardButton?.onClick.AddListener(StartGameAIVeryHard);

        startGameTwoPlayersButton?.onClick.AddListener(StartGameTwoPlayers);

        openMenuButton?.onClick.AddListener(StartMenu);
    }

    private void OnDisable()
    {
        startGameAINormalButton?.onClick.RemoveListener(StartGameAINormal);
        startGameAIHardButton?.onClick.RemoveListener(StartGameAIHard);
        startGameAIVeryHardButton?.onClick.RemoveListener(StartGameAIVeryHard);

        startGameTwoPlayersButton?.onClick.RemoveListener(StartGameTwoPlayers);

        openMenuButton?.onClick.RemoveListener(StartMenu);
    }

    private void OpenSceneMenu()
    {
        sceneChangerAnimation.OnFinishedAppear -= OpenSceneMenu;
        SceneManager.LoadScene(MENU_SCENE, LoadSceneMode.Single);
    }

    private void OpenSceneGameField() 
    {
        sceneChangerAnimation.OnFinishedAppear -= OpenSceneGameField;
        SceneManager.LoadScene(GAME_FIELD, LoadSceneMode.Single);
    }

    private void StartMenu()
    {
        AudioSystem.PlaySound(AudioSystem.inst.Click);
        SetUp.Mode = Modes.TwoPlayers;
        sceneChangerAnimation.Appear();
        sceneChangerAnimation.OnFinishedAppear += OpenSceneMenu;
    }

    private void StartGameTwoPlayers()
    {
        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        SetUp.Mode = Modes.TwoPlayers;
        sceneChangerAnimation.Appear();
        sceneChangerAnimation.OnFinishedAppear += OpenSceneGameField;
    }

    private void StartGameAINormal()
    {
        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        SetUp.Mode = Modes.AINormal;
        sceneChangerAnimation.Appear();
        sceneChangerAnimation.OnFinishedAppear += OpenSceneGameField;
    }

    private void StartGameAIHard()
    {
        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        SetUp.Mode = Modes.AIHard;
        sceneChangerAnimation.Appear();
        sceneChangerAnimation.OnFinishedAppear += OpenSceneGameField;
    }

    private void StartGameAIVeryHard()
    {
        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        SetUp.Mode = Modes.AIVeryHard;
        sceneChangerAnimation.Appear();
        sceneChangerAnimation.OnFinishedAppear += OpenSceneGameField;
    }
}
