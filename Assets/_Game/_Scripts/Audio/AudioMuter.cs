using UnityEngine;

[RequireComponent(typeof(AudioSource)), AddComponentMenu("Audio/Audio Muter Component")]
public class AudioMuter : MonoBehaviour
{
    [SerializeField] private bool isMusic = false;

    private AudioSource audioSource;
    private float baseVolume;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        baseVolume = audioSource.volume;
    }

    private void Start()
    {
        AudioSystem.inst.OnAudioSettingsChanged += _audioSettingsChanged;

        _audioSettingsChanged();
    }
    private void _audioSettingsChanged()
    {
        if (isMusic)
            audioSource.volume = (AudioSystem.settings.music) ? AudioSystem.settings.musicVolume * baseVolume : 0F;
        if (!isMusic)
            audioSource.volume = (AudioSystem.settings.sounds) ? AudioSystem.settings.soundsVolume * baseVolume : 0F;
    }

    private void OnDestroy()
    {
        AudioSystem.inst.OnAudioSettingsChanged -= _audioSettingsChanged;
    }
}
