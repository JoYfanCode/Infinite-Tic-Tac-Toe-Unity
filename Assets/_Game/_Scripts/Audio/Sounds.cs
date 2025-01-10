using System;
using UnityEngine;

[Serializable]
public class Sounds
{
    public GameObject Click => click;
    public GameObject OpenGameMode => openGameMode;
    public GameObject Win => win;
    public GameObject Firework => firework;

    [SerializeField] private GameObject click;
    [SerializeField] private GameObject openGameMode;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject firework;
}