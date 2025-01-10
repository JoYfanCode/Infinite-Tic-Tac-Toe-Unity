using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyParticleSystemAfterPlaying : MonoBehaviour
{
    [SerializeField] bool isDefinedTime;
    [SerializeField, ShowIf("isDefinedTime")] float TimeUntilDestroy = 2f;

    void Awake()
    {
        if (isDefinedTime)
        {
            Destroy(gameObject, TimeUntilDestroy);
        }
        else
        {
            Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
        }
    }
}