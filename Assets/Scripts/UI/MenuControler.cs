using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControler : MonoBehaviour
{
    public static MenuControler instance;

    private void Awake()
    {
        //单例
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public GameObject[] boards;
    public GameConfig gameConfig;
    
    public GameObject start;
    public GameObject changeBoard;
    public GameObject changeLevel;
    public GameObject quit;
    public GameObject restart;
    public GameObject mainMenu;

    public void MainMenuButton()
    {
        start.SetActive(true);
        changeBoard.SetActive(true);
        changeLevel.SetActive(true);
        quit.SetActive(true);
        restart.SetActive(false);
        mainMenu.SetActive(false);

        GameSystem.instance.ClearBoard();
        GameSystem.instance.start = false;
        GameSystem.instance.overMenu.SetActive(false);
        SoundService.instance.Play("button");
    }

    public void StartButton()
    {
        start.SetActive(false);
        changeBoard.SetActive(false);
        changeLevel.SetActive(false);
        quit.SetActive(false);
        restart.SetActive(true);
        mainMenu.SetActive(true);

        GameSystem.instance.RestartGame();
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ChangeBoardButton()
    {
        BoardControler.instance.ChangeBoard();
        SoundService.instance.Play("button");
    }

    public void ChangeLevelButton()
    {
        if (gameConfig.hasChessTypeC == false)
        {
            gameConfig.hasChessTypeC = true;
        }
        else if (gameConfig.hasChessTypeD == false)
        {
            gameConfig.hasChessTypeD = true;
        }
        else if (gameConfig.hasChessTypeE == false)
        {
            gameConfig.hasChessTypeE = true;
        }
        else
        {
            gameConfig.hasChessTypeC = false;
            gameConfig.hasChessTypeD = false;
            gameConfig.hasChessTypeE = false;
        }

        LevelBoardControler.instance.ChangeLevelIcon();
        SoundService.instance.Play("button");
    }
}
