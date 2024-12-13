using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VInspector;

public class ScenesChanger : MonoBehaviour
{
    [SerializeField] private SceneChangerAnimation sceneChangerAnimation;

    public static string MENU = "Menu";
    public static string GAME_FIELD = "Game Field";

    public async void OpenScene(string sceneName)
    {
        await sceneChangerAnimation.Appear();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
