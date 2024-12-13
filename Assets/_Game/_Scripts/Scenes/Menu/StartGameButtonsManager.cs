using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButtonsManager : MonoBehaviour
{
    [SerializeField] private ScenesChanger scenesChanger;

    [Space]

    [SerializeField] private Button startGameAINormalButton;
    [SerializeField] private Button startGameAIHardButton;
    [SerializeField] private Button startGameAIVeryHardButton;
    [SerializeField] private Button startGameTwoPlayersButton;

    private void OnEnable()
    {
        startGameAINormalButton?.onClick.AddListener(StartGameAINormal);
        startGameAIHardButton?.onClick.AddListener(StartGameAIHard);
        startGameAIVeryHardButton?.onClick.AddListener(StartGameAIVeryHard);

        startGameTwoPlayersButton?.onClick.AddListener(StartGameTwoPlayers);
    }

    private void OnDisable()
    {
        startGameAINormalButton?.onClick.RemoveListener(StartGameAINormal);
        startGameAIHardButton?.onClick.RemoveListener(StartGameAIHard);
        startGameAIVeryHardButton?.onClick.RemoveListener(StartGameAIVeryHard);

        startGameTwoPlayersButton?.onClick.RemoveListener(StartGameTwoPlayers);
    }

    private void StartGameTwoPlayers()
    {
        SetUp.GameMode = GameModes.TwoPlayers;

        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        scenesChanger.OpenScene(ScenesChanger.GAME_FIELD);
    }

    private void StartGameAINormal()
    {
        SetUp.GameMode = GameModes.OnePlayer;
        SetUp.AIDifficulty = AIDifficulties.NORMAL;

        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        scenesChanger.OpenScene(ScenesChanger.GAME_FIELD);
    }

    private void StartGameAIHard()
    {
        SetUp.GameMode = GameModes.OnePlayer;
        SetUp.AIDifficulty = AIDifficulties.HARD;

        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        scenesChanger.OpenScene(ScenesChanger.GAME_FIELD);
    }

    private void StartGameAIVeryHard()
    {
        SetUp.GameMode = GameModes.OnePlayer;
        SetUp.AIDifficulty = AIDifficulties.VERY_HARD;

        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        scenesChanger.OpenScene(ScenesChanger.GAME_FIELD);
    }
}