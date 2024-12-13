using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using VInspector;
using System.Threading.Tasks;

public class GameplayViewStandart : GameplayView
{
    [Tab("Parameters")]
    [SerializeField] private Color circleColor = Color.blue;
    [SerializeField] private Color crossColor = Color.red;
    [SerializeField] private float halfTransparentAlpha = 0.25f;
    [SerializeField] private int nextSlotClearMilisecCooldown = 100;
    [SerializeField] private int effectMilisecCooldown = 200;

    [Tab("Objects")]
    [SerializeField] private List<Slot> slots;
    [SerializeField] private Sprite cross;
    [SerializeField] private Sprite circle;
    [SerializeField] private Image turnStateImage;

    [Space]
    [Header("Effects")]
    [SerializeField] private GameObject circleSmallEffectPrefab;
    [SerializeField] private GameObject circleBigEffectPrefab;
    [SerializeField] private GameObject crossSmallEffectPrefab;
    [SerializeField] private GameObject crossBigEffectPrefab;

    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private Transform effectsParent;

    [Tab("Managers")]
    [SerializeField] private ScenesChanger scenesChanger;
    [SerializeField] private PointsHandler circlesPointsHandler;
    [SerializeField] private PointsHandler crossesPointsHandler;

    public override void Init(GameplayPresenter presenter)
    {
        base.Init(presenter);
        InitSlotsButtons();
    }

    public void OnSlotClicked(int id)
    {
        presenter.OnClotClicked(id);
        slots[id].Shaker.Shake();
        PlayClickSound();
    }

    public override void DisplayField(IReadOnlyList<SlotStates> Field)
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

    public override async Task ClearFieldAnimation()
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            await Task.Delay(nextSlotClearMilisecCooldown);

            slots[i].Image.color = Color.clear;
            slots[i].Shaker.Shake();
        }
    }

    public override void DisplayCounters(int countCirclesPoints, int countCrossesPoints)
    {
        circlesPointsHandler.SetPoints(countCirclesPoints);
        crossesPointsHandler.SetPoints(countCrossesPoints);
    }

    public override void BoomParticleSlot(int indexSlot, SlotStates slotState)
    {
        if (slotState == SlotStates.Circle)
        {
            slots[indexSlot].CircleEffect.Play();
        }
        else if (slotState == SlotStates.Cross)
        {
            slots[indexSlot].CrossEffect.Play();
        }
    }

    public override void LightDownColorSlot(int indexSlot)
    {
        LightUpColorSlots();

        slots[indexSlot].CanvasGroup.alpha = halfTransparentAlpha;
    }

    public override void LightUpColorSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].CanvasGroup.alpha = 1;
        }
    }

    public override void SetTurnState(SlotStates state)
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

    protected void ChangeTurnState()
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
    protected void InitSlotsButtons()
    {
        foreach (Slot slot in slots)
        {
            slot.Button.onClick.AddListener(delegate { OnSlotClicked(slot.Index); });
        }
    }

    protected void OnDisable()
    {
        foreach (Slot slot in slots)
        {
            slot.Button.onClick.RemoveAllListeners();
        }
    }

    public override void PlayWinEffects(SlotStates winState)
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
        rightEffect.transform.localPosition = rightPoint.transform.localPosition;
        PlayFireworkSound();

        await Task.Delay(effectMilisecCooldown);

        GameObject leftEffect = Instantiate(smallEffectPrefab, Vector2.zero, Quaternion.identity, effectsParent).gameObject;
        leftEffect.transform.localPosition = leftPoint.transform.localPosition;
        PlayFireworkSound();

        await Task.Delay(effectMilisecCooldown);

        GameObject centerEffect = Instantiate(bigEffectPrefab, Vector2.zero, Quaternion.identity, effectsParent).gameObject;
        centerEffect.transform.localPosition = centerPoint.transform.localPosition;
        PlayFireworkSound();
    }

    public override void OpenMenu()
    {
        scenesChanger.OpenScene(ScenesChanger.MENU);
    }
}
