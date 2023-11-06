using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void OnGameOver(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SceneManager.LoadScene("PlayerMovementScene");
        }
    }
}
