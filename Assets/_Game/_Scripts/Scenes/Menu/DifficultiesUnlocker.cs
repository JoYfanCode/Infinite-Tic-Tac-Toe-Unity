using System.Collections.Generic;
using UnityEngine;

public class DifficultiesUnlocker : MonoBehaviour
{
    [SerializeField] List<AIDifficulty> difficulties = new();

    public void Unlock(int levelsCompleted)
    {
        for (int i = 0; i < difficulties.Count && i <= levelsCompleted; i++)
        {
            difficulties[i].Unlock();
        }
    }

    public void UnlockLastOneWithEffect(int levelsCompleted)
    {
        difficulties[levelsCompleted].NewUnlock();
    }
}