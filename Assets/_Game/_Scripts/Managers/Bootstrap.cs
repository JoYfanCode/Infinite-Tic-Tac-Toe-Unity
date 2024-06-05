using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private bool _isAI;

    [SerializeField] private ViewUI _viewUI;
    [SerializeField] private Transform _interfaceCanvas;

    public void Awake()
    {
        //View ViewPrefab = _viewUI;

        //View view = Instantiate(ViewPrefab, _interfaceCanvas);

        View view = _viewUI;
        Model model = new Model3x3(view);
        Presenter presenter;

        if (_isAI)
        {
            presenter = new PresenterAI(model, new AINormal());
        }
        else
        {
            presenter = new PresenterTwoPlayers(model);
        }

        view.Init(presenter);
    }
}
