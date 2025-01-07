using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectsAppearAnimation<T> where T : MonoBehaviour
{
    private ObjectsAppearAnimationConfig _config;
    private IReadOnlyList<GameObject> _objects;
    private List<Vector3> _defaultScale = new();
    private List<StaticButtonScalerAnimation> _scaleAnimations = new();
    private int _activeButtonsCount = 0;

    private const int MILISEC_IN_SEC = 1000;

    public ObjectsAppearAnimation(ObjectsAppearAnimationConfig config, IReadOnlyList<T> objects)
    {
        _config = config;
        _objects = Utilities.ConverToGameObjects(objects);

        for (int i = 0; i < _objects.Count; i++)
        {
            _defaultScale.Add(_objects[i].transform.localScale);
            _objects[i].TryGetComponent(out StaticButtonScalerAnimation anim);
            _scaleAnimations.Add(anim);

            _objects[i].transform.localScale = Vector3.zero;
            if (_scaleAnimations[i]) _scaleAnimations[i].enabled = false;
        }
    }

    public async void Appear() => await AppearAsync();

    public async Task AppearAsync()
    {
        _activeButtonsCount = 0;

        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].transform.localScale = Vector3.zero;
            if (_scaleAnimations[i]) _scaleAnimations[i].enabled = false;
        }

        await Task.Delay((int)(_config.AppearCooldown * MILISEC_IN_SEC));

        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].transform.LeanScale(_defaultScale[i], _config.ScaleTime).setEaseOutBack().setOnComplete(EnableScaleAnimation);
            await Task.Delay((int)(_config.NextAppearButtonTime * MILISEC_IN_SEC));
        }
    }

    public async void Dissappear() => await DisappearAsync();

    public async Task DisappearAsync()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            _scaleAnimations[i].enabled = false;
        }

        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].transform.LeanScale(Vector3.zero, _config.ScaleTime).setEaseOutExpo();
            await Task.Delay((int)(_config.NextDisappearButtonTime * MILISEC_IN_SEC));
        }
    }

    private void EnableScaleAnimation()
    {
        if (_scaleAnimations[_activeButtonsCount]) _scaleAnimations[_activeButtonsCount].enabled = true;
        _activeButtonsCount++;
    }
}