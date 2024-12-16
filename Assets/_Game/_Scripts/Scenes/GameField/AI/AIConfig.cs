using System.Collections;
using UnityEngine;
using VInspector;

[CreateAssetMenu(fileName = "AIConfig", menuName = "Configs/AIConfig")]
public class AIConfig : ScriptableObject
{
    [Variants("OneTurn", "MiniMax")]
    [SerializeField] private string algorithm;

    [ShowIf("algorithm", "OneTurn")]
    [SerializeField, RangeResettable(0, 100)] private int percentsNoticeWinTurn = 100;
    [SerializeField, RangeResettable(0, 100)] private int percentsNoticeDontLoseTurn = 100;
    [EndIf]

    [ShowIf("algorithm", "MiniMax")]
    [SerializeField, RangeResettable(1, 10)] private int maxDepth = 2;
    [Space]
    [SerializeField, RangeResettable(1, 100)] private int percentDontLose1Depth = 100;
    [SerializeField, RangeResettable(1, 100)] private int percentDontLose2Depth = 100;
    [SerializeField, RangeResettable(1, 100)] private int percentDontLose3Depth = 100;
    [Space]
    [SerializeField, RangeResettable(1, 100)] private int percentNoticeBestTurn = 100;
    [SerializeField, RangeResettable(1, 100)] private int percentNoticeSecondBestTurn = 100;
    [Space]
    [SerializeField] private int lose1DepthInTurns = 10000;
    [SerializeField] private int lose2DepthInTurns = 10000;
    [SerializeField] private int lose3DepthInTurns = 10000;
    [SerializeField] private int lose4DepthInTurns = 10000;
    [EndIf]

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
}