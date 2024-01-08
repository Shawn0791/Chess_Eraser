using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorceControler : MonoBehaviour
{
    public static ScorceControler instance;

    public GameConfig gameConfig;
    public Text scorceBoard;
    public Text heightestBoard;

    public float reward;

    public float mainScorce;
    private float maxScorce;
    private float multiply;

    public void Awake()
    {
        instance = this;
    }

    public void ChangeMultiply()
    {
        if (gameConfig.hasChessTypeC == false)
        {
            multiply = 1;
            return;
        }
        else if (gameConfig.hasChessTypeD == false)
        {
            multiply = 2;
            return;
        }
        else if ((gameConfig.hasChessTypeE == false))
        {
            multiply = 3;
            return;
        }
        else
        {
            multiply = 4;
            return;
        }
    }

    public void AddScorce()
    {
        mainScorce += (10 + reward) * multiply;

        if (reward < 10)
            reward++;

        RefreshScorceBoard();
        TryRecordHeightScorce();
    }

    public void RefreshScorceBoard()
    {
        scorceBoard.text = mainScorce.ToString();
    }

    public void ResetScorce()
    {
        scorceBoard.text = "0";
        mainScorce = 0;
    }

    public void TryRecordHeightScorce()
    {
        //记录最高分
        if (mainScorce > maxScorce)
        {
            maxScorce = mainScorce;
            heightestBoard.text = mainScorce.ToString();
        }
    }

    public void ClearReward()
    {
        mainScorce += 200 + multiply * 200;
    }
}
