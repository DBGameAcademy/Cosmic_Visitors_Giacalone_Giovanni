using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Helper.MonoSingleton<GameManager>
{
    public enum eGameState { Idle, Start, Playing, ResetLevel, ChangeLevel, Win, Gameover}
    public eGameState gameState;

    private void OnEnable()
    {
        EventManager.Instance.StartListening("OnLevelBuilt", PlayGame);
        EventManager.Instance.StartListening("OnPlayerDead", GameOver);
        EventManager.Instance.StartListening("OnLevelCompleted", ResetLevel);
        EventManager.Instance.StartListening("OnPlayerReset", ChangeLevel);
        EventManager.Instance.StartListening("OnPlayerWin", Win);
    }

    protected override void Awake()
    {
        base.Awake();
        gameState = eGameState.Idle;
    }

    private void Update()
    {
        Debug.Log(gameState);

        switch (gameState)
        {
            case eGameState.Idle:
                
                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    StartGame();
                }

                //here we show the title screen
                
                break;

            case eGameState.Start:

                break;

            case eGameState.Playing:

                break;

            case eGameState.ChangeLevel:
                if (Input.anyKeyDown)
                {
                    EventManager.Instance.TriggerEvent("OnChangeLevel");
                }
                break;
        }
    }

    private void StartGame()
    {
        EventManager.Instance.TriggerEvent("OnGameStarted");
    }

    private void PlayGame()
    {
        gameState = eGameState.Playing;
    }

    private void GameOver()
    {
        gameState = eGameState.Gameover;
    }

    private void Win()
    {
        gameState = eGameState.Win;
    }

    private void ResetLevel()
    {
        gameState = eGameState.ResetLevel;
    }

    private void ChangeLevel()
    {
        gameState = eGameState.ChangeLevel;
    }
}
