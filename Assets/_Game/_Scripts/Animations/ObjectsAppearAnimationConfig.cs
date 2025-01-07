using UnityEngine;

[CreateAssetMenu(fileName = "ObjectsAppearAnimationConfig", menuName = "Configs/ObjectsAppearAnimationConfig")]
public class ObjectsAppearAnimationConfig : ScriptableObject
{
    [SerializeField] private float scaleTime = 0.2f;
    [SerializeField] private float nextAppearButtonTime = 0.1f;
    [SerializeField] private float nextDisappearButtonTime = 0.1f;
    [SerializeField] private float appearCooldown = 0.2f;

    public float ScaleTime => scaleTime;
    public float NextAppearButtonTime => nextAppearButtonTime;
    public float NextDisappearButtonTime => nextDisappearButtonTime;
    public float AppearCooldown => appearCooldown;
}
