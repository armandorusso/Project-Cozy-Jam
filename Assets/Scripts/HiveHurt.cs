using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HiveHurt : MonoBehaviour
{
    private Hive _hive;
    
    [SerializeField] private int _maxHealth;
    public int MaxHealth => _maxHealth;
    [SerializeField] private Animator _ghostHiveAnimator;
    [SerializeField] private int _currentHealth;
    [SerializeField] private float _hurtTime;
    [SerializeField] private float _ghostMovementSpeed;
    private float _hurtCountdown;
    private bool _isHurt;
    private bool _isGhostMoving;

    public bool IsHurt => _isHurt;
    public int CurrentHealth => _currentHealth;

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
        if (_currentHealth <= 0 && _ghostHiveAnimator.isActiveAndEnabled && _isGhostMoving)
        {
            _ghostHiveAnimator.transform.Translate(Vector3.up * _ghostMovementSpeed * Time.deltaTime);
        }
        
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

    public bool GetHurt(Vector3 enemyDirection, int damage)
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
            StartCoroutine(Die());
            return true;
        }

        return false;
    }

    private IEnumerator Die()
    {
        // Freeze all movement
        //Time.timeScale = 0.0f;
        
        // Disable hive movement and start their death animation process
        _hive.HiveMovement.IsMoving = false;
        _isHurt = false;
        _hive.Rigidbody.simulated = false;
        _hive.SpriteRendererComponent.sortingOrder = 3;
        _hive.AnimatorComponent.Play(_hive.HiveMovement.HiveDirection.x >= 0.0f ? "HurtRight" : "HurtLeft");
        
        // Disable Player movement and start their shocked animation process
        foreach (var player in GameManager.Instance.Players)
        {
            player.PlayerMovement.IsMoving = false;
            player.PlayerHurt.isHurt = false;
            player.AnimatorComponent.Play("Shock");
            player.SpriteRendererComponent.flipX = player.transform.position.x > _hive.transform.position.x; // flip sprite if the hive is on the left side of the player
            player.PlayerHurt.Rigidbody2DComponent.simulated = false;
            player.SpriteRendererComponent.sortingOrder = 3;
        }

        /*// Disable and freeze enemies on all running components
        foreach (var enemy in GameManager.Instance.EnemyWaves)
        {
            if (!enemy) continue;
            
            var enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.EnemyMovement.IsMoving = false;
            enemyComponent.EnemyAttacking.IsStriking = false;
            enemyComponent.EnemyAnimator.AnimatorComponent.enabled = false;
            enemyComponent.EnemyAnimator.SpriteRendererComponent.sortingOrder = 1;
        }*/
        
        GameManager.Instance.ResetSpritesForBackground();

        yield return new WaitForSecondsRealtime(2.0f);
        
        _hive.AnimatorComponent.Play(_hive.HiveMovement.HiveDirection.x >= 0.0f ? "DeathRight" : "DeathLeft");
        GameManager.Instance.DeathBackgroundAnimator.enabled = true;

        yield return new WaitForSecondsRealtime(3.0f);
        
        _ghostHiveAnimator.transform.SetParent(null);
        _ghostHiveAnimator.gameObject.SetActive(true);
        _ghostHiveAnimator.enabled = true;
        
        foreach (var player in GameManager.Instance.Players)
        {
            player.AnimatorComponent.SetTrigger("Cry");
        }
        yield return new WaitForSeconds(2.0f);
        _ghostHiveAnimator.Play("Float");
        _isGhostMoving = true;

        yield return new WaitForSeconds(5.0f);
        
        PlayerPrefs.SetInt("Score", GameManager.Instance._currentScore);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game Over Screen");
    }
}
