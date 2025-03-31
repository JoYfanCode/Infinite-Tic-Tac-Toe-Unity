using UnityEngine;

public class ServicesInstaller : MonoBehaviour
{
    public void Init()
    {
        ServiceLocator.AddService<LevelsStorage>(new LevelsStorage());
    }
}