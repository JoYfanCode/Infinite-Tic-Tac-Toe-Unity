using UnityEngine;
using UnityEngine.UI;

public class AIDifficulty : MonoBehaviour
{
    [SerializeField] private int levelIndex = 0;
    [SerializeField] private bool isLocked = true;

    [Space]

    [SerializeField] private Transform lockTransform;
    [SerializeField] private Transform effectsParent;
    [SerializeField] private GameObject newUnlockEffectPrefab;
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockPanel;

    public bool IsLocked => isLocked;
    public int LevelIndex => levelIndex;

    private void Awake()
    {
        button.onClick.AddListener(OnDifficultyButtonClicked);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnDifficultyButtonClicked);
    }

    private void OnDifficultyButtonClicked()
    {
        SetUp.CurrentLevelIndex = levelIndex;
    }

    public void Unlock()
    {
        isLocked = false;
        SetActivePanel();
    }

    public void NewUnlock()
    {
        Unlock();
        GameObject effect = Instantiate(newUnlockEffectPrefab, effectsParent);
        effect.transform.position = lockTransform.position;
        AudioSystem.PlaySound(AudioSystem.inst.Firework);
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