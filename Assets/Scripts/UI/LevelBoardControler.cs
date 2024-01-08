using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoardControler : MonoBehaviour
{
    public static LevelBoardControler instance;

    public GameConfig gameConfig;

    public void Awake()
    {
        instance = this;

        ChangeLevelIcon();
    }

    public void ChangeLevelIcon()
    {
        transform.GetChild(3).gameObject.SetActive(gameConfig.hasChessTypeC);
        transform.GetChild(4).gameObject.SetActive(gameConfig.hasChessTypeD);
        transform.GetChild(5).gameObject.SetActive(gameConfig.hasChessTypeE);
    }
}
