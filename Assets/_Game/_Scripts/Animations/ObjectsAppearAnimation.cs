using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectsAppearAnimation : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private float scaleTime = 0.2f;
    [SerializeField] private float nextAppearButtonTime = 0.1f;
    [SerializeField] private float nextDisappearButtonTime = 0.1f;
    [SerializeField] private float appearCooldown = 0.2f;

    private List<Vector3> defaultScale = new();
    private List<StaticButtonScalerAnimation> scaleAnimations = new();

    private int activeButtonsCount = 0;
    private const int MILISEC_IN_SEC = 1000;

    public ObjectsAppearAnimation Init<T>(IReadOnlyList<T> objects) where T : MonoBehaviour
    {
        this.objects.Clear();
        this.objects = (List<GameObject>)Utilities.ConverToGameObjects(objects);
        return Init();
    }

    public ObjectsAppearAnimation Init()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            defaultScale.Add(objects[i].transform.localScale);
            objects[i].TryGetComponent(out StaticButtonScalerAnimation anim);
            scaleAnimations.Add(anim);

            objects[i].transform.localScale = Vector3.zero;
            if (scaleAnimations[i]) scaleAnimations[i].enabled = false;
        }

        return this;
    }

    public async void Appear() => await AppearAsync();

    public async Task AppearAsync()
    {
        activeButtonsCount = 0;

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.localScale = Vector3.zero;
            if (scaleAnimations[i]) scaleAnimations[i].enabled = false;
        }

        await Task.Delay((int)(appearCooldown * MILISEC_IN_SEC));

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.LeanScale(defaultScale[i], scaleTime).setEaseOutBack().setOnComplete(EnableScaleAnimation);
            await Task.Delay((int)(nextAppearButtonTime * MILISEC_IN_SEC));
        }
    }

    public async void Dissappear() => await DisappearAsync();

    public async Task DisappearAsync()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            scaleAnimations[i].enabled = false;
        }

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.LeanScale(Vector3.zero, scaleTime).setEaseOutExpo();
            await Task.Delay((int)(nextDisappearButtonTime * MILISEC_IN_SEC));
        }
    }

    private void EnableScaleAnimation()
    {
        if (scaleAnimations[activeButtonsCount]) scaleAnimations[activeButtonsCount].enabled = true;
        activeButtonsCount++;
    }
}