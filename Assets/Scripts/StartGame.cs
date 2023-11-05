using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI StartText;


    public void OnStartGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SceneManager.LoadScene("PlayerMovementScene");
        }
    }
}
