using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Shaker))]
public class ReturnToMenuButton : MonoBehaviour
{
    private Button button;
    private Shaker shaker;

    private void Awake()
    {
        button = GetComponent<Button>();
        shaker = GetComponent<Shaker>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        AudioSystem.PlayClickSound();
        shaker.Shake();
        ScenesChanger.OpenScene(ScenesChanger.inst.menu);
    }
}