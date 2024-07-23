using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesChanger : MonoBehaviour
{
    [SerializeField] private Button _startGameAINormalButton;
    [SerializeField] private Button _startGameAIHardButton;
    [SerializeField] private Button _startGameAIVeryHardButton;

    [SerializeField] private Button _startGameTwoPlayersButton;

    [SerializeField] private Button _openMenuButton;

    private const string MENU_SCENE = "Menu";
    private const string GAME_TWO_PLAYERS_SCENE = "Game Two Players";
    private const string GAME_AI_NORMAL_SCENE = "Game AI Normal";
    private const string GAME_AI_HARD_SCENE = "Game AI Hard";
    private const string GAME_AI_VERY_HARD_SCENE = "Game AI Very Hard";

    private void Start()
    {
        _startGameAINormalButton?.onClick.AddListener(StartGameAINormal);
        _startGameAIHardButton?.onClick.AddListener(StartGameAIHard);
        _startGameAIVeryHardButton?.onClick.AddListener(StartGameAIVeryHard);

        _startGameTwoPlayersButton?.onClick.AddListener(StartGameTwoPlayers);

        _openMenuButton?.onClick.AddListener(OpenMenu);
    }

    private void OpenMenu()
        => SceneManager.LoadScene(MENU_SCENE);

    private void StartGameTwoPlayers()
        => SceneManager.LoadScene(GAME_TWO_PLAYERS_SCENE);

    private void StartGameAINormal()
        => SceneManager.LoadScene(GAME_AI_NORMAL_SCENE);

    private void StartGameAIHard()
        => SceneManager.LoadScene(GAME_AI_HARD_SCENE);

    private void StartGameAIVeryHard()
        => SceneManager.LoadScene(GAME_AI_VERY_HARD_SCENE);
}
