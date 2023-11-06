using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textField;

    private void Start()
    {
        _textField.text = $"Score: {PlayerPrefs.GetInt("Score", 0)}";
    }

    public void OnGameOver(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SceneManager.LoadScene("Title Screen");
            PlayerPrefs.SetInt("Score", 0);
            PlayerPrefs.Save();
        }
    }
}
