using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

// Put away MonoBehaviour
public class ScenesChanger : MonoBehaviour
{
    public static ScenesChanger inst;

    [FilePath(Extensions = ".unity")] public string menu;
    [FilePath(Extensions = ".unity")] public string gameField;

    public void Init()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
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
