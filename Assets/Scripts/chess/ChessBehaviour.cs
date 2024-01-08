using UnityEngine;
using System.Collections.Generic;

public class ChessBehaviour : MonoBehaviour
{
    //public static List<ChessBehaviour> instances;

    //public GameObject viewChess;
    public GameObject viewA;
    public GameObject viewB;
    public GameObject viewC;
    public GameObject viewD;
    public GameObject viewE;

    public ChessData data;
    public ChessMove move;

    public void SetSpawnChessType()
    {
        data.chessType = GetNewChessType();
        SetChessType();
    }

    public void SetChessType(ChessData.ChessType type)
    {
        data.chessType = type;
        SetChessType();
    }

    public void SetChessType()
    {
        viewA.SetActive(false);
        viewB.SetActive(false);
        viewC.SetActive(false);
        viewD.SetActive(false);
        viewE.SetActive(false);

        switch (data.chessType)
        {
            case ChessData.ChessType.A:
                viewA.SetActive(true);
                break;

            case ChessData.ChessType.B:
                viewB.SetActive(true);
                break;

            case ChessData.ChessType.C:
                viewC.SetActive(true);
                break;

            case ChessData.ChessType.D:
                viewD.SetActive(true);
                break;

            case ChessData.ChessType.E:
                viewE.SetActive(true);
                break;
        }
    }

    private ChessData.ChessType GetNewChessType()
    {
        var cfg = GameService.instance.gameConfig;

        var list = new List<ChessData.ChessType>();

        list.Add(ChessData.ChessType.A);
        list.Add(ChessData.ChessType.A);
        list.Add(ChessData.ChessType.B);
        list.Add(ChessData.ChessType.B);

        if (cfg.hasChessTypeC)
        {
            list.Add(ChessData.ChessType.C);
        }
        if (cfg.hasChessTypeD)
        {
            list.Add(ChessData.ChessType.D);
        }
        if (cfg.hasChessTypeE)
        {
            list.Add(ChessData.ChessType.E);
        }
        return list[Random.Range(0, list.Count)];
    }
}
