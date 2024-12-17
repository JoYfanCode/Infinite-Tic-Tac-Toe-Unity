using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultiesManager : MonoBehaviour
{
    [SerializeField] private List<AIDifficulty> difficulties = new();

    private bool isPrevDifComplited = false;

    public void Init()
    {
        foreach (AIDifficulties difficultyType in Enum.GetValues(typeof(AIDifficulties)))
        {
            if (isPrevDifComplited)
            {
                isPrevDifComplited = false;

                foreach (AIDifficulty difficulty in difficulties)
                {
                    if (difficulty.Type == difficultyType)
                        difficulty.Unlock();
                }
            }

            if (SetUp.AIDifficultiesCompleted[difficultyType])
            {
                isPrevDifComplited = true;
            }
        }
    }
}