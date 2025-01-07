using UnityEngine;
using UnityEngine.UI;

public class StartGameButtonsManager : MonoBehaviour
{
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
        ScenesChanger.OpenScene(ScenesChanger.scenes.GameField);
    }

    private void StartGameAINormal()
    {
        SetUp.GameMode = GameModes.OnePlayer;

        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        ScenesChanger.OpenScene(ScenesChanger.scenes.GameField);
    }

    private void StartGameAIHard()
    {
        SetUp.GameMode = GameModes.OnePlayer;

        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        ScenesChanger.OpenScene(ScenesChanger.scenes.GameField);
    }

    private void StartGameAIVeryHard()
    {
        SetUp.GameMode = GameModes.OnePlayer;

        AudioSystem.PlaySound(AudioSystem.inst.OpenGameMode);
        ScenesChanger.OpenScene(ScenesChanger.scenes.GameField);
    }
}