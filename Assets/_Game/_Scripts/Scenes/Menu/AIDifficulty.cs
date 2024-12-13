using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIDifficulty : MonoBehaviour
{
    [SerializeField] private AIDifficulties type = AIDifficulties.NORMAL;
    [SerializeField] private bool isLocked = true;
    [SerializeField] private float lockedAlpha = 0.5f;

    [Space]

    [SerializeField] private Button button;
    [SerializeField] private GameObject lockPanel;

    public bool IsLocked => isLocked;
    public AIDifficulties Type => type;

    private void Awake()
    {
        button.onClick.AddListener(SetUpDifficulty);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(SetUpDifficulty);
    }

    private void SetUpDifficulty()
    {
        SetUp.AIDifficulty = type;
    }

    public void Unlock()
    {
        isLocked = false;
        SetActivePanel();
    }

    private void SetActivePanel()
    {
        if (isLocked)
        {
            button.interactable = false;
            lockPanel.SetActive(true);
        }
        else
        {
            button.interactable = true;
            lockPanel.SetActive(false);
        }
    }

    private void OnValidate()
    {
        SetActivePanel();
    }
}