using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using VInspector;
using NUnit.Framework;

public class ViewUI : View
{
    [Tab("Clots")]

    [Header("Objects")]

    [SerializeField] private List<GameObject> _slots;
    [SerializeField] private List<Shaker> _slotsShakers;
    [SerializeField] private List<ParticleSystem> _slotsParticleCircle;
    [SerializeField] private List<ParticleSystem> _slotsParticleCross;
    [SerializeField] private Sprite _cross;
    [SerializeField] private Sprite _circle;
    [SerializeField] private Image _turnStateImage;

    [Space]
    [Header("Parameters")]

    [SerializeField] private Color _crossColor;
    [SerializeField] private Color _circleColor;
    [SerializeField] private float _halfTransparentAlpha;
    [SerializeField] private float _nextSlotClearCooldown = 0.1f;

    [Tab("Managers")]

    [SerializeField] private PointsHandler _circlesPointsHandler;
    [SerializeField] private PointsHandler _crossesPointsHandler;

    private List<Image> _slotsImage = new List<Image>();
    private List<Button> _slotsButtons = new List<Button>();
    private List<CanvasGroup> _slotsCanvasGroup = new List<CanvasGroup>();

    public override void Init(Presenter presenter)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slotsImage.Add(_slots[i].GetComponent<Image>());
            _slotsButtons.Add(_slots[i].GetComponent<Button>());
            _slotsCanvasGroup.Add(_slots[i].GetComponent<CanvasGroup>());
        }

        InitSlotsButtons();

        base.Init(presenter);
    }

    public void OnSlotClicked(int id)
    {
        _presenter.OnClotClicked(id);
        AudioSystem.inst.PlayClickSound();
    }

    public override void DisplayField(List<SlotStates> Field)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (Field[i] == SlotStates.Empty)
            {
                _slotsImage[i].color = Color.clear;
            }
            
            if (Field[i] == SlotStates.Circle)
            {
                _slotsImage[i].sprite = _circle;
                _slotsImage[i].color = _circleColor;

            }
            else if (Field[i] == SlotStates.Cross)
            {
                _slotsImage[i].sprite = _cross;
                _slotsImage[i].color = _crossColor;
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
        for (int i = 0; i < _slots.Count; ++i)
        {
            yield return new WaitForSeconds(_nextSlotClearCooldown);

            _slotsImage[i].color = Color.clear;
            _slotsShakers[i].Shake();
        }

        _presenter.ResetFieldState();
        _presenter.FirstMoveAnotherPlayer();
    }

    public override void DisplayCounters(int countCirclesPoints, int countCrossesPoints)
    {
        _circlesPointsHandler.SetPoints(countCirclesPoints);
        _crossesPointsHandler.SetPoints(countCrossesPoints);

        AudioSystem.PlaySound(AudioSystem.inst.Win);
    }

    public override void BoomParticleSlot(int indexSlot, SlotStates slotState)
    {
        if (slotState == SlotStates.Circle)
        {
            _slotsParticleCircle[indexSlot].Play();
        }
        else if (slotState == SlotStates.Cross)
        {
            _slotsParticleCross[indexSlot].Play();
        }
    }

    public override void LightDownColorSlot(int indexSlot)
    {
        LightUpColorSlots();

        _slotsCanvasGroup[indexSlot].alpha = _halfTransparentAlpha;
    }

    public override void LightUpColorSlots()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slotsCanvasGroup[i].alpha = 1;
        }
    }

    public override void SetTurnState(SlotStates state)
    {
        if (state == SlotStates.Circle)
        {
            _turnStateImage.sprite = _circle;
            _turnStateImage.color = _circleColor;
        }
        else if (state == SlotStates.Cross)
        {
            _turnStateImage.sprite = _cross;
            _turnStateImage.color = _crossColor;
        }
    }

    protected void ChangeTurnState()
    {
        if (_turnStateImage.sprite == _cross)
        {
            _turnStateImage.sprite = _circle;
            _turnStateImage.color = _circleColor;
        }
        else if (_turnStateImage.sprite == _circle)
        {
            _turnStateImage.sprite = _cross;
            _turnStateImage.color = _crossColor;
        }
    }
    protected void InitSlotsButtons()
    {
        _slotsButtons[0].onClick.AddListener(delegate { OnSlotClicked(0); });
        _slotsButtons[1].onClick.AddListener(delegate { OnSlotClicked(1); });
        _slotsButtons[2].onClick.AddListener(delegate { OnSlotClicked(2); });
        _slotsButtons[3].onClick.AddListener(delegate { OnSlotClicked(3); });
        _slotsButtons[4].onClick.AddListener(delegate { OnSlotClicked(4); });
        _slotsButtons[5].onClick.AddListener(delegate { OnSlotClicked(5); });
        _slotsButtons[6].onClick.AddListener(delegate { OnSlotClicked(6); });
        _slotsButtons[7].onClick.AddListener(delegate { OnSlotClicked(7); });
        _slotsButtons[8].onClick.AddListener(delegate { OnSlotClicked(8); });
    }
}
