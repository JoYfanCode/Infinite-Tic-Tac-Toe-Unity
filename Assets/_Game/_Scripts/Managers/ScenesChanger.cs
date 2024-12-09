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
    private const string GAME_FIELD = "Game Field";

    private void OnEnable()
    {
        _startGameAINormalButton?.onClick.AddListener(StartGameAINormal);
        _startGameAIHardButton?.onClick.AddListener(StartGameAIHard);
        _startGameAIVeryHardButton?.onClick.AddListener(StartGameAIVeryHard);

        _startGameTwoPlayersButton?.onClick.AddListener(StartGameTwoPlayers);

        _openMenuButton?.onClick.AddListener(OpenMenu);
    }

    private void OnDisable()
    {
        _startGameAINormalButton?.onClick.RemoveListener(StartGameAINormal);
        _startGameAIHardButton?.onClick.RemoveListener(StartGameAIHard);
        _startGameAIVeryHardButton?.onClick.RemoveListener(StartGameAIVeryHard);

        _startGameTwoPlayersButton?.onClick.RemoveListener(StartGameTwoPlayers);

        _openMenuButton?.onClick.RemoveListener(OpenMenu);
    }

    private void OpenMenu()
    {
        AudioSystem.PlaySound(AudioSystem.inst.Click);
        SceneManager.LoadScene(MENU_SCENE, LoadSceneMode.Single);
    }

    private void OpenGameField() 
    {
        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        SceneManager.LoadScene(GAME_FIELD, LoadSceneMode.Single);
    }

    private void StartGameTwoPlayers()
    {
        SetUp.Mode = Modes.TwoPlayers;
        OpenGameField();
    }

    private void StartGameAINormal()
    {
        SetUp.Mode = Modes.AINormal;
        OpenGameField();
    }

    private void StartGameAIHard()
    {
        SetUp.Mode = Modes.AIHard;
        OpenGameField();
    }

    private void StartGameAIVeryHard()
    {
        SetUp.Mode = Modes.AIVeryHard;
        OpenGameField();
    }
}
