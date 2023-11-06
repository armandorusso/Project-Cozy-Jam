using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;

    void Start()
    {
        _highScoreText.text = $"{PlayerPrefs.GetInt("HighScore", 0)}";
        GameManager.ScoreIncrementedAction += OnScoreIncreased;
    }

    private void OnScoreIncreased(int score)
    {
        _scoreText.text = $"{score}";
        if (GameManager.Instance._currentScore > GameManager.Instance._highScore)
        {
            _highScoreText.text = $"{score}";
            GameManager.Instance._highScore = score;
            PlayerPrefs.SetInt("HighScore", GameManager.Instance._highScore);
            PlayerPrefs.Save();
        }
    }

    private void OnDestroy()
    {
        GameManager.ScoreIncrementedAction -= OnScoreIncreased;
    }
}
