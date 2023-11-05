using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    void Start()
    {
        GameManager.ScoreIncrementedAction += OnScoreIncreased;
    }

    private void OnScoreIncreased(int score)
    {
        _scoreText.text = $"{score}";
    }

    private void OnDestroy()
    {
        GameManager.ScoreIncrementedAction -= OnScoreIncreased;
    }
}
