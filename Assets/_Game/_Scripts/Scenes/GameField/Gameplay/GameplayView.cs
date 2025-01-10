using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameplayView : MonoBehaviour
{
    [SerializeField, TabGroup("Parameters")] Color circleColor = Color.blue;
    [SerializeField, TabGroup("Parameters")] Color crossColor = Color.red;
    [SerializeField, TabGroup("Parameters")] float halfTransparentAlpha = 0.25f;
    [SerializeField, TabGroup("Parameters")] int nextSlotClearMilisecCooldown = 100;
    [SerializeField, TabGroup("Parameters")] int effectMilisecCooldown = 200;

    [SerializeField, TabGroup("Objects")] List<Slot> slots;
    [SerializeField, TabGroup("Objects")] Sprite cross;
    [SerializeField, TabGroup("Objects")] Sprite circle;
    [SerializeField, TabGroup("Objects")] Image turnStateImage;

    [SerializeField, TabGroup("Effects")] GameObject circleSlotEffectPrefab;
    [SerializeField, TabGroup("Effects")] GameObject crossSlotEffectPrefab;
    [SerializeField, TabGroup("Effects")] GameObject circleSmallEffectPrefab;
    [SerializeField, TabGroup("Effects")] GameObject circleBigEffectPrefab;
    [SerializeField, TabGroup("Effects")] GameObject crossSmallEffectPrefab;
    [SerializeField, TabGroup("Effects")] GameObject crossBigEffectPrefab;

    [Space]

    [SerializeField, TabGroup("Effects")] Transform effectLeftPoint;
    [SerializeField, TabGroup("Effects")] Transform effectCenterPoint;
    [SerializeField, TabGroup("Effects")] Transform effectRightPoint;
    [SerializeField, TabGroup("Effects")] Transform effectsParent;

    GameplayPresenter _presenter;
    AudioSystem _audioSystem;

    public IReadOnlyList<Slot> Slots => slots;

    public void Init(GameplayPresenter presenter)
    {
        _presenter = presenter;
        _audioSystem = AudioSystem.inst;
        InitSlotsButtons();
    }

    public void OnSlotClicked(int id)
    {
        _presenter.OnClotClicked(id);
        slots[id].Shaker.Shake();
        PlayClickSound();
    }

    void OnDisable()
    {
        foreach (Slot slot in slots)
        {
            slot.Button.onClick.RemoveAllListeners();
        }
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

    public void PlayClickSound() => _audioSystem.PlayClickSound();
    public void PlayWinSound() => _audioSystem.PlaySound(_audioSystem.Sounds.Win);
    public void PlayFireworkSound() => _audioSystem.PlaySound(_audioSystem.Sounds.Firework);

    void ChangeTurnState()
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

    void InitSlotsButtons()
    {
        foreach (Slot slot in slots)
        {
            slot.Button.onClick.AddListener(delegate { OnSlotClicked(slot.Index); });
        }
    }
}
