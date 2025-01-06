using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayViewStandart : GameplayView
{
    [BoxGroup("Parameters")]
    [SerializeField] private Color circleColor = Color.blue;
    [SerializeField] private Color crossColor = Color.red;
    [SerializeField] private float halfTransparentAlpha = 0.25f;
    [SerializeField] private int nextSlotClearMilisecCooldown = 100;
    [SerializeField] private int effectMilisecCooldown = 200;

    [BoxGroup("Objects")]
    [SerializeField] private List<Slot> slots;
    [SerializeField] private Sprite cross;
    [SerializeField] private Sprite circle;
    [SerializeField] private Image turnStateImage;

    [Space]
    [Header("Effects")]
    [SerializeField] private GameObject circleSlotEffectPrefab;
    [SerializeField] private GameObject crossSlotEffectPrefab;
    [SerializeField] private GameObject circleSmallEffectPrefab;
    [SerializeField] private GameObject circleBigEffectPrefab;
    [SerializeField] private GameObject crossSmallEffectPrefab;
    [SerializeField] private GameObject crossBigEffectPrefab;

    [Space]

    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private Transform effectsParent;

    [BoxGroup("Managers")]
    private PointsHandler _circlesPointsHandler;
    private PointsHandler _crossesPointsHandler;

    [Inject]
    public void Construct([Inject(Id = "Circles")] PointsHandler circlesPointsHandler,
                          [Inject(Id = "Crosses")] PointsHandler crossesPointsHandler)
    {
        _circlesPointsHandler = circlesPointsHandler;
        _crossesPointsHandler = crossesPointsHandler;
    }

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
        Permutation permutation = new Permutation(slots.Count);
        permutation.Shuffle();

        for (int i = 0; i < slots.Count; i++)
        {
            await Task.Delay(nextSlotClearMilisecCooldown);

            slots[permutation.GetElement(i)].Image.color = Color.clear;
            slots[permutation.GetElement(i)].Shaker.Shake();
        }
    }

    public override void DisplayCounters(int countCirclesPoints, int countCrossesPoints)
    {
        _circlesPointsHandler.SetPoints(countCirclesPoints);
        _crossesPointsHandler.SetPoints(countCrossesPoints);
    }

    public override void BoomParticleSlot(int indexSlot, SlotStates slotState)
    {
        GameObject slotEffectPrefab;

        if (slotState == SlotStates.Circle) slotEffectPrefab = circleSlotEffectPrefab;
        else slotEffectPrefab = crossSlotEffectPrefab;

        GameObject slotEffect = Instantiate(slotEffectPrefab, Vector2.zero, Quaternion.identity, effectsParent).gameObject;
        slotEffect.transform.localPosition = slots[indexSlot].transform.localPosition;
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

    public override async Task OpenMenuAsync()
    {
        await ScenesChanger.OpenSceneAsync(ScenesChanger.inst.menu);
    }
}
