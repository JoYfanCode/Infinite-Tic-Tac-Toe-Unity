using UnityEngine;

public class ServicesInstaller : MonoBehaviour
{
    public void Awake()
    {
        ServiceLocator.AddService<MoneyStorage>(new MoneyStorage());
    }
}