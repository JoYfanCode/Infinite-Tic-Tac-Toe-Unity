using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int Index => index;
    public Image Image => image;
    public Button Button => button;
    public CanvasGroup CanvasGroup => canvasGroup;
    public Shaker Shaker => shaker;

    [SerializeField] int index;
    [SerializeField] Image image;
    [SerializeField] Button button;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Shaker shaker;
}
