using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class MenuBootstrap : MonoBehaviour
{
    [SerializeField] private AudioSystem audioSystem;
    [SerializeField] private SaveLoader saveLoader;

    [SerializeField] private SceneChangerAnimation sceneChangerAnimation;
    [SerializeField] private ObjectsAppearAnimation startButtonsAppearAnimation;
    [SerializeField] private ObjectsAppearAnimation AIDifficultyButtonsAppearAnimation;
    [SerializeField] private DifficultiesManager difficultiesManager;

    public async void Awake()
    {
        saveLoader.Init();
        audioSystem.Init();

        
        AIDifficultyButtonsAppearAnimation.Init();
        startButtonsAppearAnimation.Init();
        
        await sceneChangerAnimation.FadeAsync();

        if (SetUp.isOpenedNewDifficulty)
        {
            difficultiesManager.UnlockWithoutNew();
            await AIDifficultyButtonsAppearAnimation.AppearAsync();
            difficultiesManager.UnlockNew();

            SetUp.isOpenedNewDifficulty = false;
        }
        else
        {
            difficultiesManager.Unlock();
            startButtonsAppearAnimation.Appear();
        }
    }
}