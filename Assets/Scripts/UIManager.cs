using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Helper.MonoSingleton<UIManager>
{
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject gameoverScreen;
    [SerializeField] private GameObject victoryLore;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject bestiary;
    [SerializeField] private GameObject bestiaryButton;

    [SerializeField] private GameObject score;
    private int scoreNumber;
    public bool doubleScore;
    public bool tripleScore;
    
    [SerializeField] private GameObject levelNumber;
    public float timeRemaining = 2;

    [SerializeField] private Image healthBarImage;
    [SerializeField] private GameObject health;
    private float maxFill;

    [SerializeField] private GameObject dialogue;
    private string[] messages;

    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject instructionsButton;

    [SerializeField] private GameObject planet;
    
    private void OnEnable()
    {
        EventManager.Instance.StartListening("OnGameStarted", PlayerUI);
        EventManager.Instance.StartListening("OnPlayerDead", GameoverScreen);
        EventManager.Instance.StartListening("OnPlayerWin", VictoryLore);
        EventManager.Instance.StartListening("OnGameEnd", VictoryScreen);
        EventManager.Instance.StartListening("OnPlayerReset", LoadingActive);
        EventManager.Instance.StartListening("OnChangeLevel", LoadingFalse);
        EventManager.Instance.StartListening("OnPlayerHit", ChangeHealthText);
        EventManager.Instance.StartListening("OnFinalLevelEntered", ShowPlanet);
    }

    private void OnDisable()
    {
        EventManager.Instance.StopListening("OnGameStarted", PlayerUI);
        EventManager.Instance.StopListening("OnPlayerDead", GameoverScreen);
        EventManager.Instance.StopListening("OnPlayerWin", VictoryLore);
        EventManager.Instance.StopListening("OnGameEnd", VictoryScreen);
        EventManager.Instance.StopListening("OnPlayerReset", LoadingActive);
        EventManager.Instance.StopListening("OnChangeLevel", LoadingFalse);
        EventManager.Instance.StopListening("OnPlayerHit", ChangeHealthText);
        EventManager.Instance.StopListening("OnFinalLevelEntered", ShowPlanet);
    }

    protected override void Awake()
    {
        base.Awake();

        maxFill = healthBarImage.fillAmount;
        UpdateScore(0);
        messages = new string[] { "I can't wait to be reunited with all my friends", "I'm also a little worried about why no one answered my communications", "Wait..", "Why the hell are they shooting at me!!" };
        doubleScore = false;
        tripleScore = false;
        titleScreen.SetActive(true);
        planet.SetActive(true);
        instructionsButton.SetActive(true);
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.eGameState.ChangeLevel)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimer(timeRemaining);
            }
            else if (timeRemaining <= 0 && Input.anyKeyDown)
            {
                EventManager.Instance.TriggerEvent("OnChangeLevel");
            }
        }
        else if (GameManager.Instance.gameState == GameManager.eGameState.Playing)
        {
            timeRemaining = 2;
        }
        else if (GameManager.Instance.gameState == GameManager.eGameState.Win)
        {
            ShowPlanet();
        }
    }

    private void TitleScreen()
    {
        if (titleScreen.activeSelf)
        {
            titleScreen.SetActive(false);
        }
        else if (!titleScreen.activeSelf)
        {
            titleScreen.SetActive(true);
        }
    }

    private void GameoverScreen()
    {
        gameoverScreen.SetActive(true);
    }

    private void VictoryLore()
    {
        victoryLore.SetActive(true);
    }

    private void VictoryScreen()
    {
        victoryLore.SetActive(false);
        victoryScreen.SetActive(true);
    }
    
    private void LoadingActive()
    {
        loadingScreen.SetActive(true);
    }

    private void LoadingFalse()
    {
        loadingScreen.SetActive(false);
    }

    private void PlayerUI()
    {
        playerUI.SetActive(true);
    }

    public void UpdateHealth(float _health, float _currentHealth)
    {
        healthBarImage.fillAmount = Mathf.Clamp(_currentHealth / _health, 0, 1f);
        health.GetComponent<TextMeshProUGUI>().text = _currentHealth.ToString() + " / " + _health.ToString();
    }

    public void ChangeHealthText()
    {
        if (healthBarImage.fillAmount <= maxFill / 2 && healthBarImage.fillAmount > maxFill / 3)
        {
            health.GetComponent<TextMeshProUGUI>().color = Color.white;
            healthBarImage.color = Color.yellow;
            doubleScore = true;
        }
        else if (healthBarImage.fillAmount <= maxFill / 3)
        {
            healthBarImage.color = Color.red;
            doubleScore = false;
            tripleScore = true;
        }
    }

    public void UpdateScore(int _score)
    {
        if (doubleScore)
        {
            scoreNumber += _score * 2;
        }
        else if (!doubleScore && tripleScore)
        {
            scoreNumber += _score * 3;
        }
        else if (!doubleScore && !tripleScore)
        {
            scoreNumber += _score;
        }
        
        score.GetComponent<TextMeshProUGUI>().text = "SCORE: " + scoreNumber;
    }

    public void UpdateTimer(float _time)
    {
        _time += 1;

        float seconds = Mathf.FloorToInt(_time % 60);

        timer.GetComponent<TextMeshProUGUI>().text = seconds.ToString();
    }

    public void UpdateLevelNumber(int _level)
    {
        if (_level <= 5)
        {
            levelNumber.GetComponent<TextMeshProUGUI>().text = "LEVEL " + _level;
        }
        else
        {
            levelNumber.GetComponent<TextMeshProUGUI>().text = "VICTORY ";
        }
    }

    private void ShowPlanet()
    {
        if (planet.activeSelf)
        {
            planet.SetActive(false);
        }
        else if (!planet.activeSelf)
        {
            planet.SetActive(true);
        }
    }
    
    public IEnumerator Opening()
    {
        TitleScreen();
        ShowPlanet();
        instructionsButton.SetActive(false);
        bestiaryButton.SetActive(false);
        dialogue.SetActive(true);
        yield return new WaitForSeconds(2);
        dialogue.GetComponent<TextMeshProUGUI>().text = messages[0];
        yield return new WaitForSeconds(2);
        dialogue.GetComponent<TextMeshProUGUI>().text = messages[1];
        yield return new WaitForSeconds(2);
        dialogue.GetComponent<TextMeshProUGUI>().text = messages[2];
        yield return new WaitForSeconds(2);
        dialogue.GetComponent<TextMeshProUGUI>().text = messages[3];
        yield return new WaitForSeconds(2);

        dialogue.SetActive(false);
        EventManager.Instance.TriggerEvent("OnGameStarted");
    }

    public void StartOpening()
    {
        EventManager.Instance.TriggerEvent("OnOpeningStarted");
        StartCoroutine(Opening());
    }

    public void ShowInstructions()
    {
        if (instructionsPanel.activeSelf)
        {
            instructionsPanel.SetActive(false);
            bestiaryButton.SetActive(true);
        }
        else if (!instructionsPanel.activeSelf)
        {
            instructionsPanel.SetActive(true);
            bestiaryButton.SetActive(false);
        }
        
        TitleScreen();
    }

    public void ShowBestiary()
    {
        if (bestiary.activeSelf)
        {
            bestiary.SetActive(false);
            titleScreen.SetActive(true);
            instructionsButton.SetActive(true);
            planet.SetActive(true);
        }
        else if (!bestiary.activeSelf)
        {
            bestiary.SetActive(true);
            titleScreen.SetActive(false);
            instructionsButton.SetActive(false);
            planet.SetActive(false);
        }
    }
}
