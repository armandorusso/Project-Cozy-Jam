using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeCongoLine : MonoBehaviour
{
    public enum BeeType
    {
        None,
        YellowBee,
        BlackBee
    }
    
    [SerializeField] private BeePoolScriptableObject BeePool;
    [SerializeField] private float SpawnInterval;
    [SerializeField] public BeeType BeeColor;
    [SerializeField] public Transform PlayerFollowPoint;
    [SerializeField] public float AngleAttackThreshold;
    [SerializeField] private Player _player;

    private Vector2 _playerDirection => _player.PlayerMovement.PlayerDirection;
    private bool _isPlayerHurt => _player.PlayerHurt.isHurt;
    private float _currentTime;
    private bool _isAttacking => BeePool.IsAttacking;
    
    public static Action<BeeAlly, int> BeeAttackAction;
    
    void Start()
    {
        BeePool.InstantiateCongoPool();
    }

    void Update()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= SpawnInterval)
        {
            BeePool.SpawnBee(PlayerFollowPoint);
            _currentTime = 0f;
        }
    }
    
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (_isPlayerHurt || _isAttacking)
            return;
        
        // Call event to trigger bee attack
        if (col.gameObject.CompareTag(BeeColor.ToString()) && BeePool.TotalBeesSpawned >= 4)
        {
            col.gameObject.TryGetComponent(out BeeAlly bee);

            if (bee != null && bee.Index >= 4)
            {
                var angleBetweenPlayerAndBee = Vector2.Dot(_playerDirection, bee.Direction);

                if (Mathf.Abs(angleBetweenPlayerAndBee) >= AngleAttackThreshold && Mathf.Abs(angleBetweenPlayerAndBee) <= 0.99f)
                {
                    Debug.Log($"Attack Triggered. Angle: {angleBetweenPlayerAndBee}");
                    BeeAttackAction?.Invoke(bee, BeePool.GetInstanceID());   
                }
            }
        }
    }

    public void OnDestroy()
    {
        BeePool.RemoveAllBees();
    }
}
