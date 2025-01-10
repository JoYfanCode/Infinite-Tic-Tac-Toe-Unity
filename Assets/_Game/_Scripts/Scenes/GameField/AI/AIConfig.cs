using Sirenix.OdinInspector;
using System;
using UnityEngine;



[CreateAssetMenu(fileName = "AIConfig", menuName = "Configs/AIConfig")]
[Serializable]
public class AIConfig : ScriptableObject
{
    public int PercentsNoticeWinTurn => percentsNoticeWinTurn;
    public int PercentsNoticeDontLoseTurn => percentsNoticeDontLoseTurn;

    public int MaxDepth => maxDepth;
    public int PercentDontLose1Depth => percentDontLose1Depth;
    public int PercentDontLose2Depth => percentDontLose2Depth;
    public int PercentDontLose3Depth => percentDontLose3Depth;
    public int Lose1DepthInTurns => lose1DepthInTurns;
    public int Lose2DepthInTurns => lose2DepthInTurns;
    public int Lose3DepthInTurns => lose3DepthInTurns;
    public int Lose4DepthInTurns => lose4DepthInTurns;
    public int PercentNoticeBestTurn => percentNoticeBestTurn;
    public int PercentNoticeSecondBestTurn => percentNoticeSecondBestTurn;

    [EnumToggleButtons, HideLabel]
    [SerializeField] private AIAlgorithm algorithm;

    [BoxGroup("OneTurn", ShowLabel = false), ShowIf("IsOneTurn")]
    [SerializeField, Range(0, 100)] private int percentsNoticeWinTurn = 100;

    [BoxGroup("OneTurn"), ShowIf("IsOneTurn")]
    [SerializeField, Range(0, 100)] private int percentsNoticeDontLoseTurn = 100;

    [BoxGroup("MiniMax", ShowLabel = false), ShowIf("IsMiniMax")]
    [SerializeField, Range(1, 10)] private int maxDepth = 2;

    [BoxGroup("MiniMax"), ShowIf("IsMiniMax")]
    [SerializeField, Range(1, 100)] private int percentDontLose1Depth = 100;

    [BoxGroup("MiniMax"), ShowIf("IsMiniMax")]
    [SerializeField, Range(1, 100)] private int percentDontLose2Depth = 100;

    [BoxGroup("MiniMax"), ShowIf("IsMiniMax")]
    [SerializeField, Range(1, 100)] private int percentDontLose3Depth = 100;

    [PropertySpace(SpaceBefore = 30)]

    [BoxGroup("MiniMax"), ShowIf("IsMiniMax")]
    [SerializeField, Range(1, 100)] private int percentNoticeBestTurn = 100;

    [BoxGroup("MiniMax"), ShowIf("IsMiniMax")]
    [SerializeField, Range(1, 100)] private int percentNoticeSecondBestTurn = 100;

    [BoxGroup("MiniMax"), ShowIf("IsMiniMax")]
    [SerializeField] private int lose1DepthInTurns = 10000;

    [BoxGroup("MiniMax"), ShowIf("IsMiniMax")]
    [SerializeField] private int lose2DepthInTurns = 10000;

    [BoxGroup("MiniMax"), ShowIf("IsMiniMax")]
    [SerializeField] private int lose3DepthInTurns = 10000;

    [BoxGroup("MiniMax"), ShowIf("IsMiniMax")]
    [SerializeField] private int lose4DepthInTurns = 10000;

    private bool IsOneTurn() => algorithm == AIAlgorithm.OneTurn;
    private bool IsMiniMax() => algorithm == AIAlgorithm.MiniMax;
}