using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyParticleSystemAfterPlaying : MonoBehaviour
{
    [SerializeField] private bool isDefinedTime;
    [ShowIf("isDefinedTime")]
    [SerializeField] private float TimeUntilDestroy = 2f;

    private void Awake()
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