using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    public Image Image => image;
    public Shaker Shaker => shaker;

    [SerializeField] Image image;
    [SerializeField] Shaker shaker;
}