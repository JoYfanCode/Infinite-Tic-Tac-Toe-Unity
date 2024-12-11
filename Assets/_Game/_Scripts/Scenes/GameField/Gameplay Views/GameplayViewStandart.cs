using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using VInspector;
using NUnit.Framework;

public class GameplayViewStandart : GameplayView
{
    [Tab("Slots")]

    [Header("Objects")]

    [SerializeField] private List<Slot> slots;
    [SerializeField] private Sprite cross;
    [SerializeField] private Sprite circle;
    [SerializeField] private Image turnStateImage;

    [Space]
    [Header("Parameters")]

    [SerializeField] private Color circleColor = Color.blue;
    [SerializeField] private Color crossColor = Color.red;
    [SerializeField] private float halfTransparentAlpha = 0.25f;
    [SerializeField] private float nextSlotClearCooldown = 0.1f;

    [Tab("Managers")]

    [SerializeField] private PointsHandler circlesPointsHandler;
    [SerializeField] private PointsHandler crossesPointsHandler;

    public override void Init(GameplayPresenter presenter)
    {
        InitSlotsButtons();
        base.Init(presenter);
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

    public override void ClearFieldAnimation()
    {
        StartCoroutine(ClearFieldAnimationCoroutine());
    }

    private IEnumerator ClearFieldAnimationCoroutine()
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            yield return new WaitForSeconds(nextSlotClearCooldown);

            slots[i].Image.color = Color.clear;
            slots[i].Shaker.Shake();
        }

        presenter.ResetFieldState();
        presenter.FirstMoveAnotherPlayer();
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
}
