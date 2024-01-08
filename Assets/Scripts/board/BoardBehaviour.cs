using UnityEngine;

public class BoardBehaviour : MonoBehaviour
{
    public ParticleSystem psStartGame;

    public void OnGameStart()
    {
        psStartGame.Play(true);
        SoundService.instance.Play("game start");
    }

}