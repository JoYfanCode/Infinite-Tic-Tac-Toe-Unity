using UnityEngine;
using UnityEngine.UI;

public class AIDifficulty : MonoBehaviour
{
    public bool IsLocked => isLocked;
    public int LevelIndex => levelIndex;

    [SerializeField] int levelIndex = 0;
    [SerializeField] bool isLocked = true;

    [Space]

    [SerializeField] Transform lockTransform;
    [SerializeField] Transform effectsParent;
    [SerializeField] GameObject newUnlockEffectPrefab;
    [SerializeField] Button button;
    [SerializeField] GameObject lockPanel;

    AudioSystem _audioSystem;

    void Awake()
    {
        _audioSystem = AudioSystem.inst;
        button.onClick.AddListener(OnDifficultyButtonClicked);
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(OnDifficultyButtonClicked);
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
        _audioSystem.PlaySound(_audioSystem.Sounds.Firework);
    }

    void OnDifficultyButtonClicked()
    {
        SetUp.CurrentLevelIndex = levelIndex;
    }


    void SetActivePanel()
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

    void OnValidate()
    {
        SetActivePanel();
    }
}