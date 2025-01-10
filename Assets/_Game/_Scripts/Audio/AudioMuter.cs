using UnityEngine;

[RequireComponent(typeof(AudioSource)), AddComponentMenu("Audio/Audio Muter Component")]
public class AudioMuter : MonoBehaviour
{
    [SerializeField] bool isMusic = false;

    AudioSystem _audioSystem;
    AudioSource _audioSource;
    float _baseVolume;

    void Awake()
    {
        _audioSystem = AudioSystem.inst;
        _audioSource = gameObject.GetComponent<AudioSource>();
        _baseVolume = _audioSource.volume;
    }

    void Start()
    {
        _audioSystem.OnAudioSettingsChanged += _audioSettingsChanged;

        _audioSettingsChanged();
    }
    void _audioSettingsChanged()
    {
        if (isMusic)
            _audioSource.volume = (_audioSystem.Settings.music) ? _audioSystem.Settings.musicVolume * _baseVolume : 0F;
        else
            _audioSource.volume = (_audioSystem.Settings.sounds) ? _audioSystem.Settings.soundsVolume * _baseVolume : 0F;
    }

    void OnDestroy()
    {
        _audioSystem.OnAudioSettingsChanged -= _audioSettingsChanged;
    }
}
