using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsHandler : MonoBehaviour
{
    public IReadOnlyList<Button> ModeButtons => modeButtons;
    public IReadOnlyList<Button> LevelsButtons => levelsButtons;

    [SerializeField] List<Button> modeButtons;
    [SerializeField] List<Button> levelsButtons;

    ObjectsAppearAnimation<Button> _modeButtonsAnimation;
    ObjectsAppearAnimation<Button> _levelsButtonsAnimation;
    AudioSystem _audioSystem;

    public void Init(ObjectsAppearAnimation<Button> modeButtonsAnimation, ObjectsAppearAnimation<Button> levelsButtonsAnimation)
    {
        _modeButtonsAnimation = modeButtonsAnimation;
        _levelsButtonsAnimation = levelsButtonsAnimation;
        _audioSystem = AudioSystem.inst;
    }

    void OnEnable()
    {
        foreach (Button button in levelsButtons)
        {
            button?.onClick.AddListener(OnLevelButtonClicked);
        }

        modeButtons[0]?.onClick.AddListener(OnOnePlayerModeButtonClicked);
        modeButtons[1]?.onClick.AddListener(OnTwoPlayerModeButtonClicked);
    }

    void OnDisable()
    {
        foreach (Button button in levelsButtons)
        {
            button?.onClick.RemoveListener(OnLevelButtonClicked);
        }

        modeButtons[0]?.onClick.RemoveListener(OnOnePlayerModeButtonClicked);
        modeButtons[1]?.onClick.RemoveListener(OnTwoPlayerModeButtonClicked);
    }

    void OnLevelButtonClicked()
    {
        SetUp.GameMode = GameModes.OnePlayer;
        _audioSystem.PlaySound(_audioSystem.Sounds.OpenGameMode);
        ScenesChanger.OpenScene(ScenesChanger.scenes.GameField);
    }

    async void OnOnePlayerModeButtonClicked()
    {
        _audioSystem.PlayClickSound();
        await _modeButtonsAnimation.DisappearAsync();
        _levelsButtonsAnimation.Appear();
    }

    void OnTwoPlayerModeButtonClicked()
    {
        SetUp.GameMode = GameModes.TwoPlayers;
        _audioSystem.PlaySound(_audioSystem.Sounds.OpenGameMode);
        ScenesChanger.OpenScene(ScenesChanger.scenes.GameField);
    }
}