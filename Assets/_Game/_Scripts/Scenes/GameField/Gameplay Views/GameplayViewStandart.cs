using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using VInspector;

public class GameplayViewStandart : GameplayView
{
    [Tab("Parameters")]
    [SerializeField] private Color circleColor = Color.blue;
    [SerializeField] private Color crossColor = Color.red;
    [SerializeField] private float halfTransparentAlpha = 0.25f;
    [SerializeField] private float nextSlotClearCooldown = 0.1f;
    [SerializeField] private float effectCooldown = 0.1f;

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
    [SerializeField] private PointsHandler circlesPointsHandler;
    [SerializeField] private PointsHandler crossesPointsHandler;

    private WaitForSeconds waitCooldownEffect;

    public override void Init(GameplayPresenter presenter)
    {
        base.Init(presenter);
        waitCooldownEffect = new WaitForSeconds(effectCooldown);
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

    public override void PlayWinEffects(SlotStates winState)
    {
        if (winState == SlotStates.Circle)
        {
            StartCoroutine(PlayWinEffectsCoroutine(circleBigEffectPrefab, circleSmallEffectPrefab));
        }
        else if (winState == SlotStates.Cross)
        {
            StartCoroutine(PlayWinEffectsCoroutine(crossBigEffectPrefab, crossSmallEffectPrefab));
        }
    }

    public IEnumerator PlayWinEffectsCoroutine(GameObject bigEffectPrefab, GameObject smallEffectPrefab)
    {
        yield return waitCooldownEffect;

        GameObject rightEffect = Instantiate(smallEffectPrefab, Vector2.zero, Quaternion.identity, effectsParent).gameObject;
        rightEffect.transform.localPosition = rightPoint.transform.localPosition;
        PlayFireworkSound();

        yield return waitCooldownEffect;

        GameObject leftEffect = Instantiate(smallEffectPrefab, Vector2.zero, Quaternion.identity, effectsParent).gameObject;
        leftEffect.transform.localPosition = leftPoint.transform.localPosition;
        PlayFireworkSound();

        yield return waitCooldownEffect;
        yield return waitCooldownEffect;

        GameObject centerEffect = Instantiate(bigEffectPrefab, Vector2.zero, Quaternion.identity, effectsParent).gameObject;
        centerEffect.transform.localPosition = centerPoint.transform.localPosition;
        PlayFireworkSound();
    }
}
