using UnityEngine;
using UnityEngine.SceneManagement; // Для перезапуска игры

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Правила игры")]
    public int starsToWin = 15; // Сколько звезд нужно для победы
    public int maxTrapHits = 5; // Сколько раз можно попасть в ловушку

    private int currentStars = 0;
    private int trapHits = 0;
    private bool isGameOver = false;

    void Awake()
    {
        // Создаем синглтон, чтобы к GameManager можно было обращаться из любых скриптов
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddStar()
    {
        if (isGameOver) return;
        currentStars++;
        Debug.Log($"Звезд собрано: {currentStars} / {starsToWin}");

        if (currentStars >= starsToWin)
        {
            WinGame();
        }
    }

    public void HitTrap()
    {
        if (isGameOver) return;
        trapHits++;
        Debug.Log($"Попаданий в ловушки: {trapHits} / {maxTrapHits}");

        if (trapHits >= maxTrapHits)
        {
            LoseGame("Ты попала в ловушки 5 раз!");
        }
    }

    public void CaughtByWitch()
    {
        if (isGameOver) return;
        LoseGame("Ведьма тебя поймала!");
    }

    private void WinGame()
    {
        isGameOver = true;
        Debug.Log("ПОБЕДА! Ты собрала все звезды!");
        // Позже здесь появится красивый экран победы
    }

    private void LoseGame(string reason)
    {
        isGameOver = true;
        Debug.Log($"ПОРАЖЕНИЕ! {reason}");
        // Позже здесь появится экран поражения
    }
}