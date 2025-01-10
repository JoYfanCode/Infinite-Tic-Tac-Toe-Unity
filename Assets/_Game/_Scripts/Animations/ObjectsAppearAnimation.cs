using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectsAppearAnimation<T> where T : MonoBehaviour
{
    readonly ObjectsAppearAnimationConfig config;
    readonly IReadOnlyList<GameObject> objects;

    List<Vector3> _defaultScale = new();
    List<StaticButtonScalerAnimation> _scaleAnimations = new();
    int _activeButtonsCount = 0;

    const int MILISEC_IN_SEC = 1000;

    public ObjectsAppearAnimation(ObjectsAppearAnimationConfig config, IReadOnlyList<T> objects)
    {
        this.config = config;
        this.objects = Utilities.ConverToGameObjects(objects);

        Init();
    }

    public async void Appear() => await AppearAsync();

    public async Task AppearAsync()
    {
        _activeButtonsCount = 0;

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.localScale = Vector3.zero;
            if (_scaleAnimations[i]) _scaleAnimations[i].enabled = false;
        }

        await Task.Delay((int)(config.AppearCooldown * MILISEC_IN_SEC));

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.LeanScale(_defaultScale[i], config.ScaleTime).setEaseOutBack().setOnComplete(EnableScaleAnimation);
            await Task.Delay((int)(config.NextAppearButtonTime * MILISEC_IN_SEC));
        }
    }

    public async void Dissappear() => await DisappearAsync();

    public async Task DisappearAsync()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            _scaleAnimations[i].enabled = false;
        }

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.LeanScale(Vector3.zero, config.ScaleTime).setEaseOutExpo();
            await Task.Delay((int)(config.NextDisappearButtonTime * MILISEC_IN_SEC));
        }
    }

    void Init()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            _defaultScale.Add(objects[i].transform.localScale);
            objects[i].TryGetComponent(out StaticButtonScalerAnimation anim);
            _scaleAnimations.Add(anim);

            objects[i].transform.localScale = Vector3.zero;
            if (_scaleAnimations[i]) _scaleAnimations[i].enabled = false;
        }
    }

    void EnableScaleAnimation()
    {
        if (_scaleAnimations[_activeButtonsCount]) _scaleAnimations[_activeButtonsCount].enabled = true;
        _activeButtonsCount++;
    }
}