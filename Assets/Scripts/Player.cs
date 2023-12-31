using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerHurt _playerHurt;
    [SerializeField] private BeeCongoLine _beeCongoLine;

    public PlayerMovement PlayerMovement => _playerMovement;
    public PlayerHurt PlayerHurt => _playerHurt;

    public BeeCongoLine BeeCongoLine => _beeCongoLine;
    
    [SerializeField] private Animator _animator;
    public Animator AnimatorComponent => _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRendererComponent => _spriteRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        if (!_playerMovement) _playerMovement = GetComponent<PlayerMovement>();
        if (!_playerHurt) _playerHurt = GetComponent<PlayerHurt>();
        if (!_beeCongoLine) _beeCongoLine = GetComponent<BeeCongoLine>();
    }
}
