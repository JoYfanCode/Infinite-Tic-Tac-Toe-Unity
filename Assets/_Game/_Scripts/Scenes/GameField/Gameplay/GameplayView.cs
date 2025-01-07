using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameplayView : MonoBehaviour
{
    [SerializeField, TabGroup("Parameters")] private Color circleColor = Color.blue;
    [SerializeField, TabGroup("Parameters")] private Color crossColor = Color.red;
    [SerializeField, TabGroup("Parameters")] private float halfTransparentAlpha = 0.25f;
    [SerializeField, TabGroup("Parameters")] private int nextSlotClearMilisecCooldown = 100;
    [SerializeField, TabGroup("Parameters")] private int effectMilisecCooldown = 200;

    [SerializeField, TabGroup("Objects")] private List<Slot> slots;
    [SerializeField, TabGroup("Objects")] private Sprite cross;
    [SerializeField, TabGroup("Objects")] private Sprite circle;
    [SerializeField, TabGroup("Objects")] private Image turnStateImage;

    [SerializeField, TabGroup("Effects")] private GameObject circleSlotEffectPrefab;
    [SerializeField, TabGroup("Effects")] private GameObject crossSlotEffectPrefab;
    [SerializeField, TabGroup("Effects")] private GameObject circleSmallEffectPrefab;
    [SerializeField, TabGroup("Effects")] private GameObject circleBigEffectPrefab;
    [SerializeField, TabGroup("Effects")] private GameObject crossSmallEffectPrefab;
    [SerializeField, TabGroup("Effects")] private GameObject crossBigEffectPrefab;

    [Space]

    [SerializeField, TabGroup("Effects")] private Transform effectLeftPoint;
    [SerializeField, TabGroup("Effects")] private Transform effectCenterPoint;
    [SerializeField, TabGroup("Effects")] private Transform effectRightPoint;
    [SerializeField, TabGroup("Effects")] private Transform effectsParent;

    private GameplayPresenter _presenter;

    public void Init(GameplayPresenter presenter)
    {
        _presenter = presenter;
        InitSlotsButtons();
    }

    public void OnSlotClicked(int id)
    {
        _presenter.OnClotClicked(id);
        slots[id].Shaker.Shake();
        PlayClickSound();
    }

    public void DisplayField(IReadOnlyList<SlotStates> Field)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (Field[i] == SlotStates.Empty)
            {
                slots[i].Image.color = Color.clear;
            }

            if (Field[i] == SlotStates.Circle)
            {
                slots[i].Image.sprite = circle;
                slots[i].Image.color = circleColor;

            }
            else if (Field[i] == SlotStates.Cross)
            {
                slots[i].Image.sprite = cross;
                slots[i].Image.color = crossColor;
            }
        }

        ChangeTurnState();
    }

    public async Task ClearFieldAnimation()
    {
        Permutation permutation = new Permutation(slots.Count);
        permutation.Shuffle();

        for (int i = 0; i < slots.Count; i++)
        {
            await Task.Delay(nextSlotClearMilisecCooldown);

            slots[permutation.GetElement(i)].Image.color = Color.clear;
            slots[permutation.GetElement(i)].Shaker.Shake();
        }
    }

    public void BoomParticleSlot(int indexSlot, SlotStates slotState)
    {
        GameObject slotEffectPrefab;

        if (slotState == SlotStates.Circle) slotEffectPrefab = circleSlotEffectPrefab;
        else slotEffectPrefab = crossSlotEffectPrefab;

        GameObject slotEffect = Instantiate(slotEffectPrefab, Vector2.zero, Quaternion.identity, effectsParent).gameObject;
        slotEffect.transform.localPosition = slots[indexSlot].transform.localPosition;
    }

    public void LightDownColorSlot(int indexSlot)
    {
        LightUpColorSlots();

        slots[indexSlot].CanvasGroup.alpha = halfTransparentAlpha;
    }

    public void LightUpColorSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].CanvasGroup.alpha = 1;
        }
    }

    public void SetTurnState(SlotStates state)
    {
        if (state == SlotStates.Circle)
        {
            turnStateImage.sprite = circle;
            turnStateImage.color = circleColor;
        }
        else if (state == SlotStates.Cross)
        {
            turnStateImage.sprite = cross;
            turnStateImage.color = crossColor;
        }
    }

    private void ChangeTurnState()
    {
        if (turnStateImage.sprite == cross)
        {
            turnStateImage.sprite = circle;
            turnStateImage.color = circleColor;
        }
        else if (turnStateImage.sprite == circle)
        {
            turnStateImage.sprite = cross;
            turnStateImage.color = crossColor;
        }
    }

    private void InitSlotsButtons()
    {
        foreach (Slot slot in slots)
        {
            slot.Button.onClick.AddListener(delegate { OnSlotClicked(slot.Index); });
        }
    }

    private void OnDisable()
    {
        foreach (Slot slot in slots)
        {
            slot.Button.onClick.RemoveAllListeners();
        }
    }

    public void PlayWinEffects(SlotStates winState)
    {
        if (winState == SlotStates.Circle)
        {
            PlayWinEffectsCoroutine(circleBigEffectPrefab, circleSmallEffectPrefab);
        }
        else if (winState == SlotStates.Cross)
        {
            PlayWinEffectsCoroutine(crossBigEffectPrefab, crossSmallEffectPrefab);
        }
    }

    public async void PlayWinEffectsCoroutine(GameObject bigEffectPrefab, GameObject smallEffectPrefab)
    {
        await Task.Delay(effectMilisecCooldown);

        GameObject rightEffect = Instantiate(smallEffectPrefab, Vector2.zero, Quaternion.identity, effectsParent).gameObject;
        rightEffect.transform.localPosition = effectRightPoint.transform.localPosition;
        PlayFireworkSound();

        await Task.Delay(effectMilisecCooldown);

        GameObject leftEffect = Instantiate(smallEffectPrefab, Vector2.zero, Quaternion.identity, effectsParent).gameObject;
        leftEffect.transform.localPosition = effectLeftPoint.transform.localPosition;
        PlayFireworkSound();

        await Task.Delay(effectMilisecCooldown);

        GameObject centerEffect = Instantiate(bigEffectPrefab, Vector2.zero, Quaternion.identity, effectsParent).gameObject;
        centerEffect.transform.localPosition = effectCenterPoint.transform.localPosition;
        PlayFireworkSound();
    }

    public async Task OpenMenuAsync()
    {
        await ScenesChanger.OpenSceneAsync(ScenesChanger.scenes.Menu);
    }

    public void PlayClickSound() => AudioSystem.PlayClickSound();
    public void PlayWinSound() => AudioSystem.PlaySound(AudioSystem.inst.Win);
    public void PlayFireworkSound() => AudioSystem.PlaySound(AudioSystem.inst.Firework);
}
