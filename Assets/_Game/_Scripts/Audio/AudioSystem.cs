using System;
using UnityEngine;

[AddComponentMenu("Audio/Audio System Component")]
public class AudioSystem : MonoBehaviour
{
    public static AudioSystem inst;

    public Action OnAudioSettingsChanged;

    public Sounds Sounds => sounds;
    public AudioSettingsModel Settings => settings;

    [SerializeField] Sounds sounds;
    AudioSettingsModel settings;

    static float _minPitch = 0.8f;
    static float _maxPitch = 1.2f;

    public void Init()
    {
        if (inst == null)
        {
            inst = this;
            settings = new AudioSettingsModel();
            DontDestroyOnLoad(inst);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void changeSoundsVolume(float value)
    {
        settings.soundsVolume = value;

        if (OnAudioSettingsChanged != null)
            OnAudioSettingsChanged();
    }

    public void changeMusicVolume(float value)
    {
        settings.musicVolume = value;

        if (OnAudioSettingsChanged != null)
            OnAudioSettingsChanged();
    }

    public void toggleSounds()
    {
        settings.sounds = !settings.sounds;

        if (OnAudioSettingsChanged != null)
            OnAudioSettingsChanged();
    }

    public void toggleMusic()
    {
        settings.music = !settings.music;

        if (OnAudioSettingsChanged != null)
            OnAudioSettingsChanged();
    }

    public void PlaySound(GameObject Sound, bool isDistortSound = true)
    {
        AudioSource sound = Instantiate(Sound).GetComponent<AudioSource>();
        DontDestroyOnLoad(sound);

        if (isDistortSound)
            sound.pitch = UnityEngine.Random.Range(_minPitch, _maxPitch);
    }

    public void PlayClickSound(bool isDistortSound = true)
    {
        AudioSource sound = Instantiate(inst.sounds.Click).GetComponent<AudioSource>();
        DontDestroyOnLoad(sound);

        if (isDistortSound)
            sound.pitch = UnityEngine.Random.Range(_minPitch, _maxPitch);
    }
}
