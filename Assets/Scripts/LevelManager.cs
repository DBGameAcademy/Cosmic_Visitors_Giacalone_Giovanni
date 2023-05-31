using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Helper.MonoSingleton<LevelManager>
{
    public bool changingLevel;
    public int levelNumber;
    public float moveDelay;
    
    private const char ALIEN = '#';
    private const char EMPTY = '*';
    
    [SerializeField]
    private List<LevelScriptable> levels;
    
    [SerializeField]
    private List<string> levelData;

    [SerializeField]
    private GameObject player;

    private void OnEnable()
    {
        EventManager.Instance.StartListening("OnGameStarted", BuildLevel);
        EventManager.Instance.StartListening("OnGameStarted", SpawnPlayer);
        EventManager.Instance.StartListening("OnChangeLevel", GoToNextLevel);
    }
    
    private void Start()
    {
        levelNumber = 1;
    }

    public void BuildLevel()
    {
        ChangeLevel();
        for (int r = 0; r < levelData.Count; r++)
        {
            for (int c = 0; c < levelData[r].Length; c++)
            {
                switch (levelData[r][c])
                {
                    case EMPTY:
                        continue;

                    case ALIEN:
                        Alien alien = AlienFactory.Instance.Create(new Vector3(c - 7, (levelData.Count - 1) - r, 0));
                        AlienManager.Instance.aliensInGame.Add(alien);
                        alien.transform.SetParent(AlienManager.Instance.transform);
                        break;
                }
            }
        }

        EventManager.Instance.TriggerEvent("OnLevelBuilt");

    }

    private void SetupLevel(LevelScriptable _level)
    {
        CheckLenght(_level);
        levelData.Clear();
        levelData.Add(_level.Row1);
        levelData.Add(_level.Row2);
        levelData.Add(_level.Row3);
        levelData.Add(_level.Row4);
        levelData.Add(_level.Row5);
        moveDelay = _level.moveDelay;
    }

    private void CheckLenght(LevelScriptable _level)
    {
        if (_level.Row1.Length > 13)
        {
            _level.Row1.Remove(14);
        }
        if (_level.Row2.Length > 13)
        {
            _level.Row2.Remove(14);
        }
        if (_level.Row3.Length > 13)
        {
            _level.Row3.Remove(14);
        }
        if (_level.Row4.Length > 13)
        {
            _level.Row4.Remove(14);
        }
        if (_level.Row5.Length > 13)
        {
            _level.Row5.Remove(14);
        }

    }
    
    private void ChangeLevel()
    {
        switch (levelNumber)
        {
            case 1:
                SetupLevel(levels[0]);
                break;
            case 2:
                SetupLevel(levels[1]);
                break;
            case 3:
                SetupLevel(levels[2]);
                break;
            case 4:
                SetupLevel(levels[3]);
                break;
        }

        // maybe this should a triggere event that the alien manager listens to and set up the moveDelay consequently (?)
        AlienManager.Instance.SetupLevel();
    }

    private void SpawnPlayer()
    {
        Instantiate(player, new Vector3(0, -4, 0), Quaternion.identity);
    }

    private void GoToNextLevel()
    {
        levelNumber++;
        if (levelNumber <= 5)
        {
            BuildLevel();
        }
        else if (levelNumber % 5 == 0)
        {
            // here we create the boss
        }
        else if (levelNumber >= 6)
        {
            EventManager.Instance.TriggerEvent("OnPlayerWin");
        }
    }
}
