using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DifficultiesManager : MonoBehaviour
{
    [SerializeField] private List<AIDifficulty> difficulties = new();
    [SerializeField] private int milisecUntilUnlockNewDif = 1000;

    private AIDifficulties newUnlockedDifficulty;
    private bool isPrevDifComplited = false;

    public void Unlock()
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

    public void UnlockWithoutNew()
    {
        DefineNewDifficulty();

        foreach (AIDifficulties difficultyType in Enum.GetValues(typeof(AIDifficulties)))
        {
            if (difficultyType == newUnlockedDifficulty)
                break;

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

    private void DefineNewDifficulty()
    {
        foreach (AIDifficulties difficultyType in Enum.GetValues(typeof(AIDifficulties)))
        {
            if (isPrevDifComplited)
            {
                newUnlockedDifficulty = difficultyType;
                isPrevDifComplited = false;
            }

            if (SetUp.AIDifficultiesCompleted[difficultyType])
            {
                isPrevDifComplited = true;
            }
        }
    }

    public async void UnlockNew()
    {
        await Task.Delay(milisecUntilUnlockNewDif);

        foreach (AIDifficulty difficulty in difficulties)
        {
            if (difficulty.Type == newUnlockedDifficulty)
                difficulty.NewUnlock();
        }
    }
}