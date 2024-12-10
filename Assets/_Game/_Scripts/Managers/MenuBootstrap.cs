using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class ManuBootstrap : MonoBehaviour
{
    [SerializeField] private AudioSystem _audioSystem;
    [SerializeField] private SceneChangerAnimation _sceneChangerAnimation;

    public void Awake()
    {
        _audioSystem.Init();
        _sceneChangerAnimation.Fade();
    }
}