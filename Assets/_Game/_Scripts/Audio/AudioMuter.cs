using UnityEngine;

[RequireComponent(typeof(AudioSource)), AddComponentMenu("Audio/Audio Muter Component")]
public class AudioMuter : MonoBehaviour
{
    [SerializeField] private bool isMusic = false;

    private AudioSource _audioSource;
    private float _baseVolume;

    private void Awake()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _baseVolume = _audioSource.volume;
    }

    private void Start()
    {
        AudioSystem.inst.OnAudioSettingsChanged += _audioSettingsChanged;

        _audioSettingsChanged();
    }
    private void _audioSettingsChanged()
    {
        if (isMusic)
            _audioSource.volume = (AudioSystem.settings.music) ? AudioSystem.settings.musicVolume * _baseVolume : 0F;
        if (!isMusic)
            _audioSource.volume = (AudioSystem.settings.sounds) ? AudioSystem.settings.soundsVolume * _baseVolume : 0F;
    }

    private void OnDestroy()
    {
        AudioSystem.inst.OnAudioSettingsChanged -= _audioSettingsChanged;
    }
}
