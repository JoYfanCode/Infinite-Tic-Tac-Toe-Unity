using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Shaker))]
public class ResetDataButton : MonoBehaviour
{
    Button _button;
    Shaker _shaker;
    AudioSystem _audioSystem;

    void Awake()
    {
        _audioSystem = AudioSystem.inst;
        _button = GetComponent<Button>();
        _shaker = GetComponent<Shaker>();
    }

    void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        _audioSystem.PlayClickSound();
        _shaker.Shake();
        PlayerPrefs.DeleteAll();
        ScenesChanger.OpenScene(ScenesChanger.scenes.Load);
    }
}
