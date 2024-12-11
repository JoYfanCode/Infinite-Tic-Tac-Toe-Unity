using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class ManuBootstrap : MonoBehaviour
{
    [SerializeField] private AudioSystem _audioSystem;
    [SerializeField] private SceneChangerAnimation _sceneChangerAnimation;
    [SerializeField] private ObjectsAppearAnimation _startButtonsAppearAnimation;
    [SerializeField] private ObjectsAppearAnimation _AIDifficultyButtonsAppearAnimation;

    public void Awake()
    {
        _audioSystem.Init();
        _AIDifficultyButtonsAppearAnimation.Init();
        _startButtonsAppearAnimation.Init().Appear();

        _sceneChangerAnimation.Fade();
    }
}