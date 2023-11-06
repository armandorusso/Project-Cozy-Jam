using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HiveHurt : MonoBehaviour
{
    private Hive _hive;
    
    [SerializeField] private int _maxHealth;
    public int MaxHealth => _maxHealth;

    [SerializeField] private int _currentHealth;
    [SerializeField] private float _hurtTime;
    private float _hurtCountdown;
    private bool _isHurt;

    public bool IsHurt => _isHurt;

    public static Action<int, int> HiveDamagedAction;
    
    // Start is called before the first frame update
    void Start()
    {
        _hive = GetComponent<Hive>();
        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isHurt) return;
        
        if (_hurtCountdown > 0)
        {
            _hurtCountdown -= Time.deltaTime;
            return;
        }
        
        if (_hurtCountdown <= 0 && !_hive.HiveMovement.IsMoving)
        {
            _hurtCountdown = 0.0f;
            _isHurt = false;
            _hive.HiveMovement.IsMoving = true;
            _hive.AnimatorComponent.SetBool("IsMoving", true);
            _hive.HiveMovement.GetHurt();
        }
    }

    public void GetHurt(Vector3 enemyDirection, int damage)
    {
        _isHurt = true;
        if (_currentHealth > damage)
        {
            if (_isHurt)
            {
                _hurtCountdown = _hurtTime;
                _hive.HiveMovement.IsMoving = false;
                _hive.AnimatorComponent.SetBool("IsMoving", false);
                _hive.AnimatorComponent.SetBool("IsCalm", false);
                _hive.Rigidbody.AddForce(100f * enemyDirection);
                _currentHealth -= damage;
                HiveDamagedAction?.Invoke(_currentHealth, _maxHealth);
                
                if (_hive.HiveMovement.HiveDirection.x > 0.0f)
                {
                    _hive.AnimatorComponent.Play("StunRight");
                }
                else if (_hive.HiveMovement.HiveDirection.x < 0.0f)
                {
                    _hive.AnimatorComponent.Play("StunLeft");
                }
            }
        }
        else
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        PlayerPrefs.SetInt("Score", GameManager.Instance._currentScore);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game Over Screen");
    }
}
