using UnityEngine;

[CreateAssetMenu(fileName = "ObjectsAppearAnimationConfig", menuName = "Configs/ObjectsAppearAnimationConfig")]
public class ObjectsAppearAnimationConfig : ScriptableObject
{
    public float ScaleTime => scaleTime;
    public float NextAppearButtonTime => nextAppearButtonTime;
    public float NextDisappearButtonTime => nextDisappearButtonTime;
    public float AppearCooldown => appearCooldown;

    [SerializeField] float scaleTime = 0.2f;
    [SerializeField] float nextAppearButtonTime = 0.1f;
    [SerializeField] float nextDisappearButtonTime = 0.1f;
    [SerializeField] float appearCooldown = 0.2f;
}
