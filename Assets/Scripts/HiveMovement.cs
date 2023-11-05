using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class HiveMovement : MonoBehaviour
{
    private Hive _hive;

    private enum BehaviorState
    {
        Calm,
        Panicked
    }
    
    [SerializeField] private Transform _hivePointToMove;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _panickedSpeed;
    [SerializeField] private float _hiveDetectionRange;
    [SerializeField] private float _calmPositionPickerTimerMin;
    [SerializeField] private float _calmPositionPickerTimerMax;
    [SerializeField] private float _calmingDownTimer;
    private Vector3 _hiveDirection;
    [SerializeField] private BehaviorState Behavior;
    [SerializeField] private float _currentSpeed;
    private float _currentCalmPositionPickerCountdown;
    private float _calmingDownCountdown;

    // Start is called before the first frame update
    void Start()
    {
        _hive = GetComponent<Hive>();
        _currentSpeed = _movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.HasGameStarted) return;
        
        if (Vector3.Distance(transform.position, _hivePointToMove.position) >= _hiveDetectionRange)
        {
            MoveHive();
        }
        else
        {
            if (Behavior == BehaviorState.Calm)
            {
                if (_currentCalmPositionPickerCountdown > 0)
                {
                    _currentCalmPositionPickerCountdown -= Time.deltaTime;
                }
                else if (_currentCalmPositionPickerCountdown <= 0)
                {
                    PickPointToMove();
                    _currentCalmPositionPickerCountdown = Random.Range(_calmPositionPickerTimerMin, _calmPositionPickerTimerMax);
                }
            }
        }
        
        if (Behavior == BehaviorState.Panicked)
        {
            if (_calmingDownCountdown > 0)
            {
                _calmingDownCountdown -= Time.deltaTime;
            }
            else if (_calmingDownCountdown <= 0)
            {
                Behavior = BehaviorState.Calm;
                _currentSpeed = _movementSpeed;
            }
        }
    }

    public void GetHurt()
    {
        _currentSpeed = _panickedSpeed;
        Behavior = BehaviorState.Panicked;
        _calmingDownCountdown = _calmingDownTimer;
        PickPointToMove();
    }
    
    private void PickPointToMove()
    {
        var pointX = Random.Range(GameManager.Instance.HiveMovementXMinimum, GameManager.Instance.HiveMovementXMaximum);
        var pointY = Random.Range(GameManager.Instance.HiveMovementYMinimum, GameManager.Instance.HiveMovementYMaximum);
        _hivePointToMove.position = new Vector3(pointX, pointY, 0.0f);
        _hiveDirection = _hivePointToMove.position - transform.position;
        _hiveDirection.Normalize();
    }
    
    private void MoveHive()
    {
        transform.Translate(_hiveDirection * (_currentSpeed * Time.deltaTime));
    }
}
