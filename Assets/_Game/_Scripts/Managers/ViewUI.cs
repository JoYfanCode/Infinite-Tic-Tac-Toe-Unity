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
    [SerializeField] private TMP_Text _youWinText;
    [SerializeField] private TMP_Text _youLoseText;

    public void OnSlotClicked(int id)
    {
        _presenter.OnClotClicked(id);
    }

    public override void DisplayField(List<SlotStates> fieldList)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (fieldList[i] == SlotStates.Empty)
            {
                _slots[i].enabled = false;
            }
            else if (fieldList[i] == SlotStates.Circle)
            {
                _slots[i].enabled = true;
                _slots[i].sprite = _circle;
            }
            else if (fieldList[i] == SlotStates.Cross)
            {
                _slots[i].enabled = true;
                _slots[i].sprite = _cross;
            }
        }
    }

    public override void DisplayYouWin()
    {
        _youWinText.gameObject.SetActive(true);
    }

    public override void DisplayYouLose()
    {
        _youLoseText.gameObject.SetActive(false);
    }
}
