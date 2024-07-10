using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private bool _isAI = false;
    [SerializeField] private AIDifficulties _AIDifficulty = AIDifficulties.AIOneTurn;
    [SerializeField] private int _maxDepth = 3;

    private enum AIDifficulties
    {
        AIOneTurn,
        AIMiniMax2Turn,
        AIMiniMax,
    }

    [SerializeField] private ViewUI _viewUI;
    [SerializeField] private Transform _interfaceCanvas;

    public void Awake()
    {
        View view = _viewUI;
        Model model = new Model3x3(view);
        Presenter presenter;

        if (_isAI == false)
        {
            presenter = new PresenterTwoPlayersSecond(model);
        }
        else if (_AIDifficulty == AIDifficulties.AIOneTurn)
        {
            presenter = new PresenterAI(model, new AIOneTurn());
        }
        else if ((_AIDifficulty == AIDifficulties.AIMiniMax2Turn))
        {
            presenter = new PresenterAI(model, new AIMiniMax(2));
        }
        else if ((_AIDifficulty == AIDifficulties.AIMiniMax))
        {
            presenter = new PresenterTwoAISecond(model, new AIMiniMaxSecond(_maxDepth));
        }
        else
        {
            presenter = new PresenterTwoPlayersSecond(model);
        }

        view.Init(presenter);
    }
}
