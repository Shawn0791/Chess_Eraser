using UnityEngine;

[CreateAssetMenu]
public class GameConfig : ScriptableObject
{
    public float animationTime_base;
    public float animationTime_interval;

    public AnimationCurve ac;

    public int startingChessCount = 10;
    public bool hasChessTypeC;
    public bool hasChessTypeD;
    public bool hasChessTypeE;
}