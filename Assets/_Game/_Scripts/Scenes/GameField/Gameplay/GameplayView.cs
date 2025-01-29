using Sirenix.OdinInspector;
using System;
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
    [SerializeField, TabGroup("Parameters"), MinMaxSlider(0, 2)] Vector2 clickSlotSoundPitchRange = new Vector2(0.6f, 1.4f);
    [SerializeField, TabGroup("Parameters")] int countWrapSounds = 30;

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

    public void OnSlotClicked(int slotId)
    {
        _presenter.OnClotClicked(slotId);
        slots[slotId].Shaker.Shake();
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

    // TODO: Special Class for patterns and more patterns

    enum ClearFieldPatterns
    {
        RIGHT_DOWN,
        LEFT_UP,
        UP_LEFT,
        RIGHT_DOWN_SPIRAL,
    }

    public async Task ClearFieldAnimation()
    {
        List<int> pattern = new();
        int countPatterns = Enum.GetValues(typeof(ClearFieldPatterns)).Length;
        int randomIndex = UnityEngine.Random.Range(0, countPatterns);

        if (randomIndex == (int)ClearFieldPatterns.RIGHT_DOWN)
        {
            pattern = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        }
        else if (randomIndex == (int)ClearFieldPatterns.LEFT_UP)
        {
            pattern = new List<int> { 8, 7, 6, 5, 4, 3, 2, 1, 0 };
        }
        else if (randomIndex == (int)ClearFieldPatterns.UP_LEFT)
        {
            pattern = new List<int> { 6, 3, 0, 7, 4, 1, 8, 5, 2 };
        }
        else if (randomIndex == (int)ClearFieldPatterns.RIGHT_DOWN_SPIRAL)
        {
            pattern = new List<int> { 0, 1, 2, 5, 8, 7, 6, 3, 4 };
        }

        for (int i = 0; i < pattern.Count; i++)
        {
            await Task.Delay(nextSlotClearMilisecCooldown);

            slots[pattern[i]].Image.color = Color.clear;
            slots[pattern[i]].Shaker.Shake();
            float minPitch = clickSlotSoundPitchRange.x;
            float maxPitch = clickSlotSoundPitchRange.y;
            float pitch = Mathf.Lerp(minPitch, maxPitch, (float)pattern[i] / (pattern.Count - 1));
            PlayClickSound(pitch);
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
    public void PlayClickSound(float pitch) => _audioSystem.PlaySound(_audioSystem.Sounds.Click, pitch);
    public void PlayWinSound() => _audioSystem.PlaySound(_audioSystem.Sounds.Win);
    public void PlayFireworkSound() => _audioSystem.PlaySound(_audioSystem.Sounds.Firework);
    //public void PlayWrapClickSound(int countTurns)
    //{
    //    float minPitch = clickSlotSoundPitchRange.x;
    //    float maxPitch = clickSlotSoundPitchRange.y;
    //    float pitch = Mathf.Lerp(minPitch, maxPitch, (float)(countTurns % countWrapSounds) / (countWrapSounds - 1));
    //    PlayClickSound(pitch);
    //}

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
