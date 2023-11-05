using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveHurt : MonoBehaviour
{
    private Hive _hive;
    
    [SerializeField] private int _maxHealth;
    public int MaxHealth => _maxHealth;

    [SerializeField] private int _currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        _hive = GetComponent<Hive>();
        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHurt(int damage)
    {
        if (_currentHealth > damage)
        {
            _currentHealth -= damage;
            _hive.HiveMovement.GetHurt();
        }
        else
        {
            _maxHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Execute Death");
        Destroy(gameObject);
    }
}
