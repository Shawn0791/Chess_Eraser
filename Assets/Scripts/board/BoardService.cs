using UnityEngine;
using System.Collections.Generic;

public class BoardService : MonoBehaviour
{
    public static BoardService instance;

    public Transform roundCenter;
    public float radius;

    public ChessBehaviour prefabChess;

    public SlotBehaviour center;

    public List<SlotBehaviour> ring1;//small 6
    public List<SlotBehaviour> ring2;//middle 12
    public List<SlotBehaviour> ring3;//big 18

    public List<SlotBehaviour> axis1;
    public List<SlotBehaviour> axis2;
    public List<SlotBehaviour> axis3;

    public List<SlotBehaviour> spawnArea;
    public List<SlotBehaviour> allArea;

    public SlotBehaviour eraseTarget;

    public bool stepAxis;
    public bool stepRing;
    private int _testAxis = 0;
    private int _testRing = 0;

    private SlotBehaviour _current;

    private void Update()
    {
        if (stepAxis)
        {
            stepAxis = false;
            axis1[_testAxis].gameObject.SetActive(false);
            axis2[_testAxis].gameObject.SetActive(false);
            axis3[_testAxis].gameObject.SetActive(false);
            _testAxis++;
        }
        if (stepRing)
        {
            stepRing = false;
            ring1[_testRing].gameObject.SetActive(false);
            ring2[_testRing].gameObject.SetActive(false);
            ring3[_testRing].gameObject.SetActive(false);
            _testRing++;
        }
    }

    int ringComparer(SlotBehaviour c1, SlotBehaviour c2)
    {
        var x1 = c1.transform.position.x;
        var x2 = c2.transform.position.x;
        var z1 = c1.transform.position.z + 0.5f;
        var z2 = c2.transform.position.z + 0.5f;
        if (Mathf.Approximately(x1, 0))
            x1 = 0;
        if (Mathf.Approximately(x2, 0))
            x2 = 0;
        if (Mathf.Approximately(z1, 0))
            z1 = 0;
        if (Mathf.Approximately(z2, 0))
            z2 = 0;
        if (Mathf.Atan2(z1, x1) < Mathf.Atan2(z2, x2))
        {
            return 1;
        }
        if (Mathf.Atan2(z1, x1) > Mathf.Atan2(z2, x2))
        {
            return -1;
        }
        return 0;
    }

    int axisComparer(SlotBehaviour c1, SlotBehaviour c2)
    {
        var x1 = c1.transform.position.x;
        var x2 = c2.transform.position.x;
        var z1 = c1.transform.position.z;
        var z2 = c2.transform.position.z;
        if (x1 - z1 > x2 - z2)
        {
            return 1;
        }
        if (x1 - z1 < x2 - z2)
        {
            return -1;
        }

        return 0;
    }

    private void Awake()
    {
        instance = this;
    }

    public void InitSlots()
    {
        allArea = new List<SlotBehaviour>();
        allArea.Add(center);
        allArea.InsertRange(0, ring1);
        allArea.InsertRange(0, ring2);
        allArea.InsertRange(0, ring3);

        spawnArea = new List<SlotBehaviour>();
        spawnArea.InsertRange(0, allArea);

        axis1 = new List<SlotBehaviour>();
        axis2 = new List<SlotBehaviour>();
        axis3 = new List<SlotBehaviour>();

        foreach (var c in allArea)
        {
            var radian = Mathf.Atan2(c.transform.position.x, c.transform.position.z) / Mathf.PI;
            // Debug.Log(c.gameObject.name + " " + radian);
            if (radian < 0)
            {
                radian += 1;
            }
            if (Mathf.Approximately(radian, 0.5f) || c == center)
            {
                axis1.Add(c);
            }
            if (Mathf.Approximately(radian, 5f / 6f) || c == center)
            {
                axis2.Add(c);
            }
            if (Mathf.Approximately(radian, 1f / 6f) || c == center)
            {
                axis3.Add(c);
            }
        }

        //re-order
        // ring1.Sort(ringComparer);
        // ring2.Sort(ringComparer);
        // ring3.Sort(ringComparer);
        axis1.Sort(axisComparer);
        axis2.Sort(axisComparer);
        axis3.Sort(axisComparer);
    }

    public void ResetAllSlots()
    {
        foreach (var slot in allArea)
            slot.ResetState();
    }

    public void SetCurrentSlot(SlotBehaviour target)
    {
        GameSystem.instance.gameState = GameSystem.GameState.ChessClicked;
        ResetAllSlots();
        target.SetCurrentTarget(true);
        DisplayGoals(target);
        _current = target;

        //计算是否有可消除目标
        var count = ShowAndGetErasableChesses();
        if (count != 0)
        {
            _current.SetCanErase(true);
        }

        SoundService.instance.Play("select");
    }

    public void DisplayGoals(SlotBehaviour target)
    {
        var chess = target.chess;
        //Debug.Log("DisplayGoals " + chess.data.chessType);
        bool cw = true;
        foreach (var slot in axis1)
        {
            if (CanReachThoughRingOrAxis(target, slot, axis1, false, ref cw))
                slot.SetClickableGoal(true);
        }

        foreach (var slot in axis2)
        {
            if (CanReachThoughRingOrAxis(target, slot, axis2, false, ref cw))
                slot.SetClickableGoal(true);
        }

        foreach (var slot in axis3)
        {
            if (CanReachThoughRingOrAxis(target, slot, axis3, false, ref cw))
                slot.SetClickableGoal(true);
        }

        foreach (var slot in ring1)
        {
            if (CanReachThoughRingOrAxis(target, slot, ring1, true, ref cw))
                slot.SetClickableGoal(true);
        }

        foreach (var slot in ring2)
        {
            if (CanReachThoughRingOrAxis(target, slot, ring2, true, ref cw))
                slot.SetClickableGoal(true);
        }

        foreach (var slot in ring3)
        {
            if (CanReachThoughRingOrAxis(target, slot, ring3, true, ref cw))
                slot.SetClickableGoal(true);
        }
    }

    private bool CanReachThoughRingOrAxis(SlotBehaviour chess, SlotBehaviour goal, List<SlotBehaviour> list, bool canLoop, ref bool cw)
    {
        var indexChess = list.IndexOf(chess);
        var indexGoal = list.IndexOf(goal);
        if (indexChess > -1 && indexGoal > -1)
        {
            //Debug.Log("canReachThoughRingOrAxis " + list.Count);
            //Debug.Log("indexes chess-goal " + indexChess + "-" + indexGoal);
            //Debug.Log(indexChess != indexGoal);
            //Debug.Log(goal.chess == null);
            //Debug.Log(isConnected(list, indexChess, indexGoal, canLoop));
            return indexChess != indexGoal
                && goal.chess == null
                 && IsConnected(list, indexChess, indexGoal, canLoop, ref cw);
        }

        return false;
    }

    public bool IsConnected(List<SlotBehaviour> list, int indexA, int indexB, bool canLoop, ref bool cw)
    {
        bool connectedCw = true;
        bool connectedAcw = true;
        int deltaCw = 0;
        int deltaAcw = 0;
        for (int i = 1; i < list.Count; i++)
        {
            int tpIndex = indexA + i;
            deltaCw = i;
            if (tpIndex >= list.Count)
            {
                if (!canLoop)
                {
                    connectedCw = false;
                    break;
                }
                else
                {
                    tpIndex -= list.Count;//ring can loop once
                }
            }
            if (tpIndex == indexB || tpIndex == indexA)
            {
                break;
            }
            var tpSlot = list[tpIndex];
            if (tpSlot.chess != null)
            {
                //Debug.Log("tpIndex " + tpIndex + " type" + tpSlot.chess.data.chessType);
                connectedCw = false;
                break;
            }
        }

        for (int i = 1; i < list.Count; i++)
        {
            int tpIndex = indexA - i;
            deltaAcw = i;
            if (tpIndex < 0)
            {
                if (!canLoop)
                {
                    connectedAcw = false;
                    break;
                }
                else
                {
                    tpIndex += list.Count;//ring can loop once
                }
            }
            if (tpIndex == indexB || tpIndex == indexA)
            {
                break;
            }
            var tpSlot = list[tpIndex];
            if (tpSlot.chess != null)
            {
                //Debug.Log("tpIndex " + tpIndex + " type" + tpSlot.chess.data.chessType);
                connectedAcw = false;
                break;
            }
        }
        //Debug.Log("isConnected " + connectedCw + " - " + connectedAcw);
        if (connectedCw && connectedAcw)
        {
            if (deltaAcw > deltaCw)
            {
                cw = true;
            }
            else
            {
                cw = false;
            }
        }
        else
        {
            if (connectedCw)
            {
                cw = true;
            }
            else if (connectedAcw)
            {
                cw = false;
            }
        }
        return connectedCw || connectedAcw;
    }

    public void SetClickableGoalSlot(SlotBehaviour goal)
    {
        //Debug.Log("SetClickableGoalSlot");
        SoundService.instance.Play("select");
        Debug.Log("will move " + _current.gameObject.name + " to " + goal.gameObject.name);
        bool suc = false;
        bool cw = true;
        if (!suc && CanReachThoughRingOrAxis(_current, goal, axis1, false, ref cw))
        {
            MoveChess(_current, goal, axis1, false, cw);
            suc = true;
        }
        if (!suc && CanReachThoughRingOrAxis(_current, goal, axis2, false, ref cw))
        {
            MoveChess(_current, goal, axis2, false, cw);
            suc = true;
        }
        if (!suc && CanReachThoughRingOrAxis(_current, goal, axis3, false, ref cw))
        {
            MoveChess(_current, goal, axis3, false, cw);
            suc = true;
        }
        if (!suc && CanReachThoughRingOrAxis(_current, goal, ring1, true, ref cw))
        {
            MoveChess(_current, goal, ring1, true, cw);
            suc = true;
        }
        if (!suc && CanReachThoughRingOrAxis(_current, goal, ring2, true, ref cw))
        {
            MoveChess(_current, goal, ring2, true, cw);
            suc = true;
        }
        if (!suc && CanReachThoughRingOrAxis(_current, goal, ring3, true, ref cw))
        {
            MoveChess(_current, goal, ring3, true, cw);
            suc = true;
        }

        if (suc)
        {
            GameSystem.instance.gameState = GameSystem.GameState.Moving;
            ResetAllSlots();
        }
        else
        {
            Debug.Log("can not move there");
        }
    }

    private void MoveChess(SlotBehaviour from, SlotBehaviour to, List<SlotBehaviour> list, bool isRing, bool cw, bool isRevert = false)
    {
        var chess = from.chess;
        int indexFrom = list.IndexOf(from);
        int indexTo = list.IndexOf(to);
        int delta = Mathf.Abs(indexFrom - indexTo);
        var cfg = GameService.instance.gameConfig;
        if (!isRing)
        {
            //is axis
            var timeAxis = cfg.animationTime_base + cfg.animationTime_interval * delta;
            chess.move.MoveAlongAxis(from.transform.position, to.transform.position, timeAxis, () =>
            {
                if (isRevert)
                {
                    Revert(to, from);
                }
                else
                {
                    MoveEnd(from, to, list, isRing, cw);
                }
            });
            return;
        }

        //isRing
        if (cw)
        {
            delta = indexTo - indexFrom;
            if (delta < 0)
            {
                delta += list.Count;
            }
        }
        else
        {
            delta = indexFrom - indexTo;
            if (delta < 0)
            {
                delta += list.Count;
            }
        }
        var timeRing = cfg.animationTime_base + cfg.animationTime_interval * delta;
        chess.move.MoveAlongRing(from.transform.position, to.transform.position, cw, timeRing, () =>
        {
            if (isRevert)
            {
                Revert(to, from);
            }
            else
            {
                MoveEnd(from, to, list, isRing, cw);
            }
        });
    }

    private void MoveEnd(SlotBehaviour from, SlotBehaviour to, List<SlotBehaviour> list, bool isRing, bool cw)
    {
        //消除
        GameSystem.instance.gameState = GameSystem.GameState.Erase;

        //两个格子slot交接棋子chess
        var chess = from.chess;
        to.ReceiveChess(chess);
        from.ReleaseChess();
        _current = to;
        SoundService.instance.Play("moved");
        //计算是否有可消除目标
        var count = ShowAndGetErasableChesses();
        if (count == 0)
        {
            MoveChess(to, from, list, isRing, !cw, true);
            //无消除目标则清零奖励
            ScorceControler.instance.reward = 0;
        }
        else
        {
            _current.SetCanErase(true);

            SpawnChess();//消除成功，生成棋子
            SpawnChess();
        }
    }

    void SpawnChess()
    {
        var suc = GameSystem.instance.TrySpawnChess();
        if (!suc)
        {
            Debug.Log("成功移动增加棋子但是棋盘满了，是否应结束游戏？");
        }
    }

    void Revert(SlotBehaviour from, SlotBehaviour to)
    {
        var chess = to.chess;
        from.ReceiveChess(chess);
        to.ReleaseChess();

        GameSystem.instance.gameState = GameSystem.GameState.Wait;
        ResetAllSlots();
        _current = null;

        SpawnChess();//消除成功，生成棋子
        SpawnChess();
    }

    int ShowAndGetErasableChesses()
    {
        int count = 0;
        foreach (var slot in allArea)
        {
            if (IsNearbySameTypeChess(_current, slot))
            {
                slot.SetCanErase(true);
                count++;
            }
        }

        return count;
    }

    private void EraseChesses()
    {
        eraseTarget.Erase();
        _current.Erase();
        _current = eraseTarget = null;
        GameSystem.instance.gameState = GameSystem.GameState.Wait;
        ResetAllSlots();
        Debug.Log("Erase Suc!");
        SoundService.instance.Play("erase");
        //加分
        ScorceControler.instance.AddScorce();
        //场上是否还存在棋子
        Invoke("CheckChessExist", 0.2f);
    }

    private bool InSameAxisOrRing(SlotBehaviour a, SlotBehaviour b)
    {
        bool bAxis1 = axis1.IndexOf(a) > -1 && axis1.IndexOf(b) > -1;
        bool bAxis2 = axis2.IndexOf(a) > -1 && axis2.IndexOf(b) > -1;
        bool bAxis3 = axis3.IndexOf(a) > -1 && axis3.IndexOf(b) > -1;
        bool bRing1 = ring1.IndexOf(a) > -1 && ring1.IndexOf(b) > -1;
        bool bRing2 = ring2.IndexOf(a) > -1 && ring2.IndexOf(b) > -1;
        bool bRing3 = ring3.IndexOf(a) > -1 && ring3.IndexOf(b) > -1;

        return bAxis1 || bAxis2 || bAxis3 || bRing1 || bRing2 || bRing3;
    }

    public void SetEraseSlot(SlotBehaviour target)
    {
        eraseTarget = target;
        //判断是否可消除
        if (IsNearbySameTypeChess(_current, eraseTarget))
        {
            EraseChesses();
            return;
        }
        //无法消除则改为选定该目标
        SetCurrentSlot(eraseTarget);

        eraseTarget = null;
        Debug.Log("Erase Fail");
    }

    bool IsNearbySameTypeChess(SlotBehaviour a, SlotBehaviour b)
    {
        if (a != b && a.chess != null && b.chess != null
          && a.chess.data.chessType == b.chess.data.chessType //两个棋子是一个类型
          && Vector3.Distance(a.transform.position, b.transform.position) < 1.3f //两个棋子间距小于1.3
          && InSameAxisOrRing(a, b))
        {
            return true;
        }

        return false;
    }

    private void CheckChessExist()
    {
        //如果不存在，则spwan
        for (int i = 0; i < spawnArea.Count; i++)
        {
            Debug.Log(i);
            if (spawnArea[i].chess != null)
                return;
            else if (i == spawnArea.Count - 1)
            {
                //奖励分数
                ScorceControler.instance.ClearReward();
                GameSystem.instance.WellDone();
                //重新刷出棋子
                int count = GameService.instance.gameConfig.startingChessCount;
                for (int c = 0; c < count; c++)
                {
                    GameSystem.instance.TrySpawnChess();
                }
            }
        }
    }
}