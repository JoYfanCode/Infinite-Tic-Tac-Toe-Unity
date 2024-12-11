using System;
using UnityEngine;

[AddComponentMenu("Audio/Audio System Component")]
public class AudioSystem : MonoBehaviour
{
    public static AudioSystem inst;
    public static AudioSettingsModel settings = null;

    [Header("Sound")]
    public GameObject Click;
    public GameObject OpenGameMode;
    public GameObject Win;

    public Action OnAudioSettingsChanged;

    private static float minPitch = 0.8f;
    private static float maxPitch = 1.2f;

    public void Init()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(inst);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (settings == null)
            settings = new AudioSettingsModel();
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

    public static void PlaySound(GameObject Sound, bool isDistortSound = true)
    {
        AudioSource sound = Instantiate(Sound).GetComponent<AudioSource>();
        DontDestroyOnLoad(sound);

        if (isDistortSound)
            sound.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
    }

    public void PlayClickSound(bool isDistortSound = true)
    {
        AudioSource sound = Instantiate(Click).GetComponent<AudioSource>();
        DontDestroyOnLoad(sound);

        if (isDistortSound)
            sound.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
    }
}
