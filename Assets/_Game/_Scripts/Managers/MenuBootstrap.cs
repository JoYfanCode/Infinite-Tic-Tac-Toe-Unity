using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class ManuBootstrap : MonoBehaviour
{
    [SerializeField] private AudioSystem _audioSystem;

    public void Awake()
    {
        _audioSystem.Init();
    }
}