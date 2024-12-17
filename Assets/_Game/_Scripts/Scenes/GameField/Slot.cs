using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

internal class Slot : MonoBehaviour
{
    public int Index;

    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Shaker shaker;

    public Image Image => image;
    public Button Button => button;
    public CanvasGroup CanvasGroup => canvasGroup;
    public Shaker Shaker => shaker;
}
