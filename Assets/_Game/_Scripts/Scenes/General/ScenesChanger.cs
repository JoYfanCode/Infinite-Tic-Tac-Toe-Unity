using System;
using System.Threading.Tasks;
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
        await sceneChangerAnimation.AppearAsync();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public async Task OpenSceneAsync(string sceneName)
    {
        await sceneChangerAnimation.AppearAsync();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
