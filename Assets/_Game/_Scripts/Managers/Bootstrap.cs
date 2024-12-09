using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Modes _mode = Modes.TwoPlayers;
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
        _mode = SetUp.Mode;

        View view = _viewUI;
        Model model = new Model3x3(view);
        Presenter presenter = CreatePresenter(_mode, model, view);

        view.Init(presenter);
    }

    private Presenter CreatePresenter(Modes mode, Model model, View view)
    {
        if (_mode == Modes.TwoPlayers) return new PresenterTwoPlayers(model, view, _restartGameCooldown);

        else if (_mode == Modes.AINormal) return new PresenterAI(model, view, new AIOneTurn(), _AICooldownMin, _AICooldownMax, _restartGameCooldown);

        else if (_mode == Modes.AIHard) return new PresenterAI(model, view, new AIMiniMax(AI_NORMAL_DEPTH), _AICooldownMin, _AICooldownMax, _restartGameCooldown);

        else if (_mode == Modes.AIVeryHard) return new PresenterAI(model, view, new AIMiniMax(AI_HARD_DEPTH), _AICooldownMin, _AICooldownMax, _restartGameCooldown);

        else if ((_mode == Modes.TwoAI)) return new PresenterTwoAI(model, view, new AIMiniMax(_AIDepth), _AICooldownMin, _AICooldownMax, _restartGameCooldown);

        else if ((_mode == Modes.TwoAIFast)) return new PresenterTwoAIFast(model, view, new AIMiniMax(_AIDepth), _restartGameCooldown);

        else return new PresenterTwoPlayers(model, view, _restartGameCooldown);
    }
}