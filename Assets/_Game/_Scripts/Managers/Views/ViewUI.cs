using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ViewUI : View
{
    [SerializeField] private List<Image> _slots;
    [SerializeField] private Sprite _cross;
    [SerializeField] private Sprite _circle;
    [SerializeField] private TMP_Text _winCircleText;
    [SerializeField] private TMP_Text _winCrossText;

    public void OnSlotClicked(int id)
    {
        _presenter.OnClotClicked(id);
    }

    public override void DisplayField(List<SlotStates> Field)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (Field[i] == SlotStates.Empty)
            {
                _slots[i].enabled = false;
            }
            else if (Field[i] == SlotStates.Circle)
            {
                _slots[i].enabled = true;
                _slots[i].sprite = _circle;
            }
            else if (Field[i] == SlotStates.Cross)
            {
                _slots[i].enabled = true;
                _slots[i].sprite = _cross;
            }
        }
    }

    public override void DisplayWinCircle()
    {
        _winCircleText.gameObject.SetActive(true);
    }

    public override void DisplayWinCross()
    {
        _winCrossText.gameObject.SetActive(true);
    }
}
