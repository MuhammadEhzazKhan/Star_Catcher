using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("Assign Menu Panels")]
    public GameObject homeMenu;
    public GameObject playArea;
    public GameObject optionsMenu;
    public GameObject pausedMenu;
    public GameObject aboutMenu;
    public GameObject gameOverMenu;

    [Header("Gameplay Prefabs")]
    public GameObject starPrefab;
    public GameObject basketPrefab;

    [Header("Gameplay Settings")]
    public float minX = -4.5f;
    public float maxX = 4.5f;
    public float spawnInterval = 1.5f;
    public float starDestroyTime = 10f;

    [Header("UI Elements")]
    public Text scoreText;
    public Text livesText;
    public Text gameOverScoreText;
    public Text highScoreText;

    private Stack<GameObject> menuHistory = new Stack<GameObject>();
    private GameObject currentMenu;
    private GameObject basketInstance;

    private int score = 0;
    private int currentLives;
    public int maxLives = 3;

    private bool isGameActive = false;
    private int highScore = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        playArea.SetActive(false);
        optionsMenu.SetActive(false);
        pausedMenu.SetActive(false);
        aboutMenu.SetActive(false);
        gameOverMenu.SetActive(false);

        Time.timeScale = 1f;
        ShowHomeMenu();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            ShowPausedMenu();
        }
    }

    private void SetActivePanel(GameObject panelToActivate)
    {
        if (panelToActivate == null) return;

        if (currentMenu != null && currentMenu != panelToActivate)
        {
            menuHistory.Push(currentMenu);
            currentMenu.SetActive(false);
        }

        panelToActivate.SetActive(true);
        currentMenu = panelToActivate;
    }

    public void ShowHomeMenu()
    {
        ClearHistory();
        SetActivePanel(homeMenu);
        ResetGame();
    }

    public void ShowPlayArea()
    {
        SetActivePanel(playArea);
        StartGameLogic();
    }

    public void ShowOptionsMenu()
    {
        SetActivePanel(optionsMenu);
    }

    public void ShowPausedMenu()
    {
        SetActivePanel(pausedMenu);
        PauseGame();
    }

    public void ShowAboutMenu()
    {
        SetActivePanel(aboutMenu);
    }

    public void ResumeButton()
    {
        ResumeGame();
        SetActivePanel(playArea);
    }

    public void ReplayButton()
    {
        ResetGame();
        StartGameLogic();
        SetActivePanel(playArea);
    }

    public void GoBack()
    {
        if (menuHistory.Count > 0)
        {
            currentMenu.SetActive(false);
            GameObject previousMenu = menuHistory.Pop();
            previousMenu.SetActive(true);
            currentMenu = previousMenu;

            if (previousMenu == playArea)
            {
                ResumeGame();
            }
        }
        else
        {
            Debug.Log("Already at Home menu");
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void ReplayGame()
    {
        ResetGame();
        ShowPlayArea();
    }

    private void StartGameLogic()
    {
        score = 0;
        currentLives = maxLives;
        UpdateScoreUI();
        UpdateLivesUI();

        Time.timeScale = 1f;
        isGameActive = true;

        SpawnBasket();
        CancelInvoke("SpawnStar");
        InvokeRepeating("SpawnStar", 1f, spawnInterval);
    }

    private void ResetGame()
    {
        CancelInvoke("SpawnStar");
        DestroyAllStars();
        DestroyBasket();
        isGameActive = false;
        score = 0;
        UpdateScoreUI();
        currentLives = maxLives;
        UpdateLivesUI();
    }

    private void SpawnBasket()
    {
        basketInstance = Instantiate(basketPrefab, new Vector3(0f, -3.7f, 0f), Quaternion.identity);
    }

    private void DestroyBasket()
    {
        if (basketInstance != null)
            Destroy(basketInstance);
    }

    private void SpawnStar()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, 9f, 0f);
        GameObject star = Instantiate(starPrefab, spawnPos, Quaternion.identity);
        StartCoroutine(StarTimeout(star));
    }

    private IEnumerator StarTimeout(GameObject star)
    {
        yield return new WaitForSeconds(starDestroyTime);

        if (star != null)
        {
            Destroy(star);
            LoseLife();
        }
    }

    private void DestroyAllStars()
    {
        GameObject[] stars = GameObject.FindGameObjectsWithTag("star");
        foreach (GameObject star in stars)
        {
            Destroy(star);
        }
    }

    public void IncrementScore()
    {
        score++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + currentLives;
    }

    private void LoseLife()
    {
        if (!isGameActive) return;

        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        CancelInvoke("SpawnStar");
        DestroyAllStars();
        DestroyBasket();
        isGameActive = false;
        Time.timeScale = 0f;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        if (gameOverScoreText != null)
            gameOverScoreText.text = "Your Score: " + score;

        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore;

        if (gameOverMenu != null)
            SetActivePanel(gameOverMenu);
        else
            ShowHomeMenu();

        //Debug.Log("Game Over!");
    }

    private void ClearHistory()
    {
        menuHistory.Clear();
    }
}
