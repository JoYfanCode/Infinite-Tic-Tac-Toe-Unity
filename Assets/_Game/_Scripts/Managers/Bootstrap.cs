using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private enum Modes
    {
        TwoPlayers,
        AINormal,
        AIHard,
        AIVeryHard,
        TwoAI,
        TwoAIFast,
    }

    [SerializeField] private Modes _AIDifficulty = Modes.TwoPlayers;
    [SerializeField] private int _AIDepth = 3;
    [SerializeField, Range(0, 1000)] private float _AICooldownMin;
    [SerializeField, Range(0, 2000)] private float _AICooldownMax;
    [SerializeField, Range(0, 5000)] private float _restartGameCooldown;

    [SerializeField] private ViewUI _viewUI;
    [SerializeField] private Transform _interfaceCanvas;

    private const int AI_NORMAL_DEPTH = 2;
    private const int AI_HARD_DEPTH = 3;

    public void Awake()
    {
        View view = _viewUI;
        Model model = new Model3x3(view);
        Presenter presenter = new PresenterTwoPlayers(model, _restartGameCooldown);

        if (_AIDifficulty == Modes.TwoPlayers)
        {
            presenter = new PresenterTwoPlayers(model, _restartGameCooldown);
        }
        else if (_AIDifficulty == Modes.AINormal)
        {
            presenter = new PresenterAI(model, new AIOneTurn(), _AICooldownMin, _AICooldownMax, _restartGameCooldown);
        }
        else if (_AIDifficulty == Modes.AIHard)
        {
            presenter = new PresenterAI(model, new AIMiniMax(AI_NORMAL_DEPTH), _AICooldownMin, _AICooldownMax, _restartGameCooldown);
        }
        else if (_AIDifficulty == Modes.AIVeryHard)
        {
            presenter = new PresenterAI(model, new AIMiniMax(AI_HARD_DEPTH), _AICooldownMin, _AICooldownMax, _restartGameCooldown);
        }
        else if ((_AIDifficulty == Modes.TwoAI))
        {
            presenter = new PresenterTwoAI(model, new AIMiniMax(_AIDepth), _AICooldownMin, _AICooldownMax, _restartGameCooldown);
        }
        else if ((_AIDifficulty == Modes.TwoAIFast))
        {
            presenter = new PresenterTwoAIFast(model, new AIMiniMax(_AIDepth), _restartGameCooldown);
        }

        view.Init(presenter);
    }
}
