using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerHurt _playerHurt;

    public PlayerMovement PlayerMovement => _playerMovement;
    public PlayerHurt PlayerHurt => _playerHurt;

    // Start is called before the first frame update
    private void Start()
    {
        if (!_playerMovement) _playerMovement = GetComponent<PlayerMovement>();
        if (!_playerHurt) _playerHurt = GetComponent<PlayerHurt>();
    }
}
