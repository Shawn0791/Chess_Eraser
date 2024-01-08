using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
//game flow control is here

public class GameSystem : MonoBehaviour
{
    public static GameSystem instance;
    public Text timeNum;
    public Text overScorce;
    public GameObject overMenu;
    public GameObject pauseMenu;
    public Animator welldone;
    public bool start;
    public float timer;

    private bool pause;

    public void Awake()
    {
        instance = this;
    }
    public GameState gameState;
    public enum GameState
    {
        None,
        Wait,
        ChessClicked,
        Moving,
        Erase,
    }

    private void Start()
    {
        gameState = GameState.None;
    }

    private void Update()
    {
        if (start)
            GameTimer();
        PauseGame();
    }

    public void RestartGame()
    {
        ClearBoard();
        //倒计时重置
        timer = 120;
        start = true;
        overMenu.SetActive(false);

        int count = GameService.instance.gameConfig.startingChessCount;
        for (int i = 0; i < count; i++)
        {
            TrySpawnChess();
        }

        gameState = GameState.Wait;
        SoundService.instance.Play("start game");
        //选定分数倍率
        ScorceControler.instance.ChangeMultiply();
    }

    public void ClearBoard()
    {
        Debug.Log("ClearBoard");
        foreach (var c in BoardService.instance.allArea)
        {
            c.RemoveChess();
            c.ResetState();
        }
        //清除分数
        ScorceControler.instance.ResetScorce();
    }

    public void TestSpawnChess()
    {
        TrySpawnChess();
    }

    public bool TrySpawnChess()
    {
        var suc = false;
        var emptySlots = new List<SlotBehaviour>();
        foreach (var c in BoardService.instance.spawnArea)
        {
            if (c.chess == null)
            {
                emptySlots.Add(c);
            }
        }

        if (emptySlots.Count > 0)
        {
            var spawnChess = emptySlots[Random.Range(0, emptySlots.Count)];
            spawnChess.Spawn();
            suc = true;
        }

        return suc;
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pause) 
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            pause = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && pause)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            pause = false;
        }
    }

    private void GameTimer()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else if (timer <= 0)
        {
            timer = 0;
            start = false;
            GameOver();
        }
        timeNum.text = Mathf.Floor(timer).ToString();
    }

    public void GameOver()
    {
        overScorce.text = ScorceControler.instance.mainScorce.ToString();
        overMenu.SetActive(true);
    }

    public void WellDone()
    {
        welldone.SetTrigger("welldone");
    }
}
