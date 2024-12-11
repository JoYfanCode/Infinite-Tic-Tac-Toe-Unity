using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Shaker shaker;

    public Image Image => image;
    public Shaker Shaker => shaker;
}