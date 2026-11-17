using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Правила игры")]
    public int starsToWin = 15;
    public int maxTrapHits = 5;

    [Header("UI Элементы")]
    public GameObject menuPanel;
    public GameObject resultPanel;
    public GameObject restartButton;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;

    private int currentStars = 0;
    private int trapHits = 0;

    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (menuPanel != null) menuPanel.SetActive(true);
        if (resultPanel != null) resultPanel.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        if (scoreText != null) scoreText.gameObject.SetActive(false); // Скрываем при старте

        Time.timeScale = 0f; // Пауза для меню
        UpdateScoreUI();
    }

    public void StartGame()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        if (scoreText != null) scoreText.gameObject.SetActive(true); // Включаем при старте игры
        Time.timeScale = 1f;
    }

    public void AddStar()
    {
        if (isGameOver) return;
        currentStars++;
        UpdateScoreUI();
        if (currentStars >= starsToWin) WinGame();
    }

    public void HitTrap()
    {
        if (isGameOver) return;
        trapHits++;
        UpdateScoreUI();
        if (trapHits >= maxTrapHits) LoseGame("Ты попала в ловушки 5 раз!");
    }

    public void CaughtByWitch()
    {
        if (isGameOver) return;
        LoseGame("Ведьма тебя поймала!");
    }

    private void WinGame()
    {
        isGameOver = true;
        if (resultPanel != null) resultPanel.SetActive(true);
        if (restartButton != null) restartButton.SetActive(true);
        if (resultText != null) resultText.text = "ПОБЕДА!\nТы собрала все звезды!";
    }

    private void LoseGame(string reason)
    {
        isGameOver = true;
        if (resultPanel != null) resultPanel.SetActive(true);
        if (restartButton != null) restartButton.SetActive(true);
        if (resultText != null) resultText.text = $"ПОРАЖЕНИЕ!\n{reason}";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Звезды: {currentStars} / {starsToWin}\nТучи: {trapHits} / {maxTrapHits}";
        }
    }
}