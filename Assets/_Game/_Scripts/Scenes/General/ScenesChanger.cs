using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public static class ScenesChanger
{
    public static ScenesConfig scenes;

    public static void Init(ScenesConfig scenesConfig)
    {
        scenes = scenesConfig;
    }

    public static async void OpenScene(string sceneName)
    {
        await SceneChangerAnimation.inst.AppearAsync();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public static async Task OpenSceneAsync(string sceneName)
    {
        await SceneChangerAnimation.inst.AppearAsync();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
