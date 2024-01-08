using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardControler : MonoBehaviour
{
    public static BoardControler instance;

    public GameConfig gameConfig;

    private int selectNum;

    public void Awake()
    {
        instance = this;
    }

    public void ChangeBoard()
    {
        if (selectNum < transform.childCount - 1) 
            selectNum++;
        else
            selectNum = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (selectNum == i)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
