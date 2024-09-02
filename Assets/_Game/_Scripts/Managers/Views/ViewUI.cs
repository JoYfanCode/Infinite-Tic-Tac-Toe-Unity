using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ViewUI : View
{
    [SerializeField] private List<GameObject> _slots;
    [SerializeField] private Sprite _cross;
    [SerializeField] private Sprite _circle;

    [SerializeField] private Color _crossColor;
    [SerializeField] private Color _circleColor;

    [SerializeField] private Image _turnStateImage;

    [SerializeField] private TMP_Text _winCircleText;
    [SerializeField] private TMP_Text _winCrossText;

    [SerializeField] private TMP_Text _counterWinsCircleText;
    [SerializeField] private TMP_Text _counterWinsCrossText;
    [SerializeField] private TMP_Text _countTurnsText;
    [SerializeField] private TMP_Text _averageTurnsText;
    [SerializeField] private TMP_Text _medianaTurnsText;


    private List<Image> _slotsImage = new List<Image>();
    private List<Button> _slotsButtons = new List<Button>();

    public override void Init(Presenter presenter)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slotsImage.Add(_slots[i].GetComponent<Image>());
            _slotsButtons.Add(_slots[i].GetComponent<Button>());
        }

        InitSlotsButtons();

        base.Init(presenter);

        _presenter.OnGameOver += UpdateAverageText;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        _presenter.OnGameOver -= UpdateAverageText;
    }

    public void OnSlotClicked(int id)
    {
        _presenter.OnClotClicked(id);
    }

    public override void DisplayField(List<SlotStates> Field, int CountTurns)
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

        _countTurnsText.text = CountTurns.ToString();
    }

    public override void DisplayWinCircle(int countWins)
    {
        _winCircleText.gameObject.SetActive(true);
        _counterWinsCircleText.text = countWins.ToString();
    }

    public override void DisplayWinCross(int countWins)
    {
        _winCrossText.gameObject.SetActive(true);
        _counterWinsCrossText.text = countWins.ToString();
    }

    public override void ClearDisplayWin()
    {
        _winCircleText.gameObject.SetActive(false);
        _winCrossText.gameObject.SetActive(false);
    }

    public void UpdateAverageText(List<int> turnsList)
    {
        int SumTurns = 0;

        foreach (int turns in turnsList)
        {
            SumTurns += turns;
        }

        _averageTurnsText.text = (Mathf.Round(SumTurns / turnsList.Count)).ToString();
        _medianaTurnsText.text = (Mathf.Round(turnsList[turnsList.Count / 2])).ToString();
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

    public override void ChangeTurnState(List<SlotStates> Field, int CountTurns)
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
    private void InitSlotsButtons()
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
