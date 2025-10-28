using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Blaster blaster;
    private Millipede millipede;
    private MushroomField mushroomField;

    public GameObject gameOver;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI highScoreText;

    private int highScore = 0;
    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        blaster = FindObjectOfType<Blaster>();
        millipede = FindObjectOfType<Millipede>();
        mushroomField = FindObjectOfType<MushroomField>();

        highScore = HighScoreManager.GetHighScore();
        UpdateHighScoreUI();
        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.GetKey(KeyCode.Space))
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);

        blaster.Respawn();
        millipede.Respawn();
        mushroomField.Clear();
        mushroomField.Generate();
        gameOver.SetActive(false);
        UpdateHighScoreUI();
    }

    public void ResetRound()
    {
        SetLives(lives - 1);

        if (lives <= 0)
        {
            GameOver();
            return;
        }

        mushroomField.Heal();
        millipede.Respawn();
        blaster.Respawn();
    }

    private void GameOver()
    {
        SoundManager.I.Play(SoundEvent.GameOver);
        gameOver.SetActive(true);
        blaster.gameObject.SetActive(false);

        if (HighScoreManager.TrySetHighScore(score))
        {
            highScore = score;
            UpdateHighScoreUI();
        }
    }

    public void IncreaseScore(int amount)
    {
        SetScore(score + amount);
    }

    public void NextLevel()
    {
        SoundManager.I.Play(SoundEvent.LevelUp);
        millipede.speed *= 1.1f;
        millipede.Respawn();
    }

    private void SetScore(int value)
    {
        score = value;
        scoreText.text = score.ToString();
    }

    private void SetLives(int value)
    {
        lives = Mathf.Max(value, 0);
        livesText.text = lives.ToString();
    }

    private void UpdateHighScoreUI()
    {
        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {highScore}";
        }
    }
}
