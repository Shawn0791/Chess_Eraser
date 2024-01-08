using UnityEngine;
using System.Collections;

public class GameService : MonoBehaviour
{
    public GameConfig gameConfig;

    public static GameService instance;

    private void Awake()
    {
        instance = this;
    }
}
