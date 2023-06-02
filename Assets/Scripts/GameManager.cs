using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Helper.MonoSingleton<GameManager>
{
    public enum eGameState { Idle, Start, Playing, ResetLevel, ChangeLevel, Win, Gameover}
    public eGameState gameState;

    private List<GameObject> laserInScene = new List<GameObject>();

    private void OnEnable()
    {
        EventManager.Instance.StartListening("OnLevelBuilt", PlayGame);
        EventManager.Instance.StartListening("OnPlayerDead", GameOver);
        EventManager.Instance.StartListening("OnPlayerDead", RemoveLaserFromScene);
        EventManager.Instance.StartListening("OnLevelCompleted", ResetLevel);
        EventManager.Instance.StartListening("OnLevelCompleted", RemoveLaserFromScene);
        EventManager.Instance.StartListening("OnPlayerReset", ChangeLevel);
        EventManager.Instance.StartListening("OnPlayerWin", Win);
    }

    private void OnDisable()
    {
        EventManager.Instance.StopListening("OnLevelBuilt", PlayGame);
        EventManager.Instance.StopListening("OnPlayerDead", GameOver);
        EventManager.Instance.StopListening("OnPlayerDead", RemoveLaserFromScene);
        EventManager.Instance.StopListening("OnLevelCompleted", ResetLevel);
        EventManager.Instance.StopListening("OnLevelCompleted", RemoveLaserFromScene);
        EventManager.Instance.StopListening("OnPlayerReset", ChangeLevel);
        EventManager.Instance.StopListening("OnPlayerWin", Win);
    }

    protected override void Awake()
    {
        base.Awake();
        gameState = eGameState.Idle;
    }

    private void Update()
    {

        switch (gameState)
        {
            case eGameState.Win:
                
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    EventManager.Instance.TriggerEvent("OnGameEnd");
                }
                break;
        }
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

    public void AddLaser(GameObject _laser)
    {
        laserInScene.Add(_laser);
    }

    private void RemoveLaserFromScene()
    {
        for (int i = laserInScene.Count - 1; i >= 0; i--)
        {
            LaserFactory.Instance.ReturnObject(laserInScene[i]);
        }
    }
}
