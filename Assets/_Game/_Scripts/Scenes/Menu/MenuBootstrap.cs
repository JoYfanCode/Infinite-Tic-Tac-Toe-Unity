using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class MenuBootstrap : MonoBehaviour
{
    [SerializeField] private AudioSystem audioSystem;
    [SerializeField] private SceneChangerAnimation sceneChangerAnimation;
    [SerializeField] private ObjectsAppearAnimation startButtonsAppearAnimation;
    [SerializeField] private ObjectsAppearAnimation AIDifficultyButtonsAppearAnimation;

    public void Awake()
    {
        audioSystem.Init();
        AIDifficultyButtonsAppearAnimation.Init();
        startButtonsAppearAnimation.Init().Appear();

        sceneChangerAnimation.Fade();
    }
}