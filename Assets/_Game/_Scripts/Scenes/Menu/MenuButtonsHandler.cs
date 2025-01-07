using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsHandler : MonoBehaviour
{
    public IReadOnlyList<Button> ModeButtons => modeButtons;
    public IReadOnlyList<Button> LevelsButtons => levelsButtons;

    [SerializeField] private List<Button> modeButtons;
    [SerializeField] private List<Button> levelsButtons;

    private ObjectsAppearAnimation<Button> _modeButtonsAnimation;
    private ObjectsAppearAnimation<Button> _levelsButtonsAnimation;

    public void Init(ObjectsAppearAnimation<Button> modeButtonsAnimation, ObjectsAppearAnimation<Button> levelsButtonsAnimation)
    {
        _modeButtonsAnimation = modeButtonsAnimation;
        _levelsButtonsAnimation = levelsButtonsAnimation;
    }

    private void OnEnable()
    {
        foreach (Button button in levelsButtons)
        {
            button?.onClick.AddListener(OnLevelButtonClicked);
        }

        modeButtons[0]?.onClick.AddListener(OnOnePlayerModeButtonClicked);
        modeButtons[1]?.onClick.AddListener(OnTwoPlayerModeButtonClicked);
    }

    private void OnDisable()
    {
        foreach (Button button in levelsButtons)
        {
            button?.onClick.RemoveListener(OnLevelButtonClicked);
        }

        modeButtons[0]?.onClick.RemoveListener(OnOnePlayerModeButtonClicked);
        modeButtons[1]?.onClick.RemoveListener(OnTwoPlayerModeButtonClicked);
    }

    private void OnLevelButtonClicked()
    {
        SetUp.GameMode = GameModes.OnePlayer;
        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        ScenesChanger.OpenScene(ScenesChanger.scenes.GameField);
    }

    private async void OnOnePlayerModeButtonClicked()
    {
        AudioSystem.PlayClickSound();
        await _modeButtonsAnimation.DisappearAsync();
        _levelsButtonsAnimation.Appear();
    }

    private void OnTwoPlayerModeButtonClicked()
    {
        SetUp.GameMode = GameModes.TwoPlayers;
        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        ScenesChanger.OpenScene(ScenesChanger.scenes.GameField);
    }
}