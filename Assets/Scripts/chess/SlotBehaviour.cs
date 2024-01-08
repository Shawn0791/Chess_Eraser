using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotBehaviour : MonoBehaviour
{
    public static List<SlotBehaviour> instances;

    public GameObject viewCurrentTarget;
    public GameObject viewCanClick;
    public GameObject viewCanErase;
    public GameObject viewMultiPushFrom;
    public GameObject viewMultiPushTo;

    public ParticleSystem psErase;
    public ParticleSystem psSpawn;

    private bool _isCurrentTarget = false;
    private bool _isClickableGoal = false;
    private bool _isErasable= false;
    private bool _isPushFrom = false;
    private bool _isPushTo = false;

    public ChessBehaviour chess { get; private set; }

    void Awake()
    {
        if (instances == null)
            instances = new List<SlotBehaviour>();
        instances.Add(this);
    }

    public void ReleaseChess()
    {
        chess = null;
    }

    public void ReceiveChess(ChessBehaviour c)
    {
        chess = c;
        chess.transform.parent = transform;
        chess.transform.localPosition = Vector3.zero;
        Debug.Log("ReceiveChess");
    }

    public void RemoveChess()
    {
        if (chess == null)
            return;

        GameObject.Destroy(chess.gameObject);
    }

    public void CreateChess()
    {
        BoardService.instance.prefabChess.gameObject.SetActive(true);
        chess = Instantiate(BoardService.instance.prefabChess, this.transform);
        chess.transform.position = this.transform.position;
        BoardService.instance.prefabChess.gameObject.SetActive(false);
    }

    public void SetState(bool isCurrentTarget, bool isClickableGoal, bool isErasable, bool isPushFrom, bool isPushTo)
    {
        SetCurrentTarget(isCurrentTarget);
        SetClickableGoal(isClickableGoal);
        SetCanErase(isErasable);
        SetPushFrom(isPushFrom);
        SetPushTo(isPushTo);
    }

    public void SetCurrentTarget(bool b)
    {
        _isCurrentTarget = b;
        viewCurrentTarget.SetActive(_isCurrentTarget);
    }

    public void SetClickableGoal(bool b)
    {
        _isClickableGoal = b;
        viewCanClick.SetActive(_isClickableGoal);
    }

    public void SetCanErase(bool b)
    {
        _isErasable = b;
        viewCanErase.SetActive(_isErasable);
    }
    
    public void SetPushFrom(bool b)
    {
        _isPushFrom = b;
        viewMultiPushFrom.SetActive(_isPushFrom);
    }

    public void SetPushTo(bool b)
    {
        _isPushTo = b;
        viewMultiPushTo.SetActive(_isPushTo);
    }

    public void ResetState()
    {
        SetState(false, false, false, false,false);
    }

    public void Spawn()
    {
        psSpawn.Play(true);
        CreateChess();
        chess.SetSpawnChessType();
        ResetState();
    }

    public void Erase()
    {
        psErase.Play(true);
        RemoveChess();
    }

    public void OnClick()
    {
        Debug.Log("OnClick slot " + gameObject.name);
        switch (GameSystem.instance.gameState)
        {
            case GameSystem.GameState.None:
                //nothing to dp
                break;
            case GameSystem.GameState.ChessClicked:
                TrySetToClickableGoal();
                break;
            case GameSystem.GameState.Moving:
                //nothing to dp
                break;
            case GameSystem.GameState.Wait:
                TrySetToCurrentTarget();
                break;
            case GameSystem.GameState.Erase:
                TrySetEraseChess();
                break;
        }
    }

    private void TrySetToClickableGoal()
    {
        // Debug.Log("TrySetToClickableGoal");
        if (chess != null)
        {
            //尝试消除
            BoardService.instance.SetEraseSlot(this);

            //BoardService.instance.SetCurrentSlot(this);
            return;
        }
          

        Debug.Log("SetToClickableGoal");
        BoardService.instance.SetClickableGoalSlot(this);
    }

    public void TrySetToCurrentTarget()
    {
        // Debug.Log("TrySetToCurrentTarget");
        if (chess == null)
            return;

        Debug.Log("SetCurrentTarget");
        BoardService.instance.SetCurrentSlot(this);
    }

    public void TrySetEraseChess()
    {
        if (chess == null)
            return;

        Debug.Log("TrySetEraseChess");
        BoardService.instance.SetEraseSlot(this);
    }
}
