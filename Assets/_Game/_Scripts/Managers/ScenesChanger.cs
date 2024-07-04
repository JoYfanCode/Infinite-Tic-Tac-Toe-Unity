using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesChanger : MonoBehaviour
{
    [SerializeField] private Button _startGameAIButton;
    [SerializeField] private Button _startGameTwoPlayersButton;
    [SerializeField] private Button _openMenuButton;

    private const string MENU_SCENE = "Menu";
    private const string GAME_TWO_PLAYERS_SCENE = "Game Two Players";
    private const string GAME_AI_SCENE = "Game AI";

    private void Start()
    {
        _startGameAIButton?.onClick.AddListener(StartGameAI);
        _startGameTwoPlayersButton?.onClick.AddListener(StartGameTwoPlayers);
        _openMenuButton?.onClick.AddListener(OpenMenu);
    }

    private void OpenMenu()
        => SceneManager.LoadScene(MENU_SCENE);

    private void StartGameTwoPlayers()
        => SceneManager.LoadScene(GAME_TWO_PLAYERS_SCENE);

    private void StartGameAI()
        => SceneManager.LoadScene(GAME_AI_SCENE);
}
