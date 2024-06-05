using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesChanger : MonoBehaviour
{
    private const string MENU_SCENE = "Menu";
    private const string GAME_TWO_PLAYERS_SCENE = "Game Two Players";
    private const string GAME_AI_SCENE = "Game AI";

    public void OpenMenu()
        => SceneManager.LoadScene(MENU_SCENE);

    public void StartGameTwoPlayers()
        => SceneManager.LoadScene(GAME_TWO_PLAYERS_SCENE);

    public void StartGameAI()
        => SceneManager.LoadScene(GAME_AI_SCENE);
}
