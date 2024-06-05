using UnityEngine.SceneManagement;

public static class Scenes
{
    private const string MENU_SCENE = "Menu";
    private const string GAME_AI_SCENE = "Game AI";
    private const string GAME_TWO_PLAYERS_SCENE = "Game Two Players";

    public static void OpenMenu()
        => SceneManager.LoadScene(MENU_SCENE);

    public static void OpenGameAI()
        => SceneManager.LoadScene(GAME_AI_SCENE);

    public static void OpenGameTwoPlayers()
        => SceneManager.LoadScene(GAME_TWO_PLAYERS_SCENE);
}
