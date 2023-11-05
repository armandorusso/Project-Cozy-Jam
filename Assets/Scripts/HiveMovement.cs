using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HiveMovement : MonoBehaviour
{
    public Hive _hive;

    public enum BehaviorState
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
    public Vector3 HiveDirection => _hiveDirection;
    [SerializeField] private BehaviorState Behavior;
    [SerializeField] private float _currentSpeed;
    private float _currentCalmPositionPickerCountdown;
    private float _calmingDownCountdown;
    public bool IsMoving;

    public static Action<BehaviorState> BehaviorChangeAction;

    // Start is called before the first frame update
    private void Start()
    {
        _hive = GetComponent<Hive>();
        _currentSpeed = _movementSpeed;
        _currentCalmPositionPickerCountdown = Random.Range(_calmPositionPickerTimerMin, _calmPositionPickerTimerMax);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameManager.Instance.HasGameStarted || _hive.HiveHurt.IsHurt) return;

        if (Vector3.Distance(transform.position, _hivePointToMove.position) >= _hiveDetectionRange)
        {
            if (!IsMoving) return;
            MoveHive();
        }
        else
        {
            UpdateDirectionBoolean();
            IsMoving = false;
            _hive.AnimatorComponent.SetBool("IsMoving", IsMoving);
            if (Behavior == BehaviorState.Calm)
            {
                if (_currentCalmPositionPickerCountdown > 0)
                {
                    _currentCalmPositionPickerCountdown -= Time.deltaTime;
                }
                else if (_currentCalmPositionPickerCountdown <= 0)
                {
                    IsMoving = true;
                    PickPointToMove();
                    _currentCalmPositionPickerCountdown = Random.Range(_calmPositionPickerTimerMin, _calmPositionPickerTimerMax);
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
                    BehaviorChangeAction?.Invoke(Behavior);
                    _currentSpeed = _movementSpeed;
                    _hive.AnimatorComponent.SetBool("IsCalm", true);
                    UpdateDirectionBoolean();
                }
            }
        }
    }

    public void GetHurt()
    {
        _hive.Rigidbody.velocity = Vector3.zero;
        _currentSpeed = _panickedSpeed;
        Behavior = BehaviorState.Panicked;
        BehaviorChangeAction?.Invoke(Behavior);
        _calmingDownCountdown = _calmingDownTimer;
        _currentCalmPositionPickerCountdown = Random.Range(_calmPositionPickerTimerMin, _calmPositionPickerTimerMax);
        PickPointToMove();
    }
    
    private void PickPointToMove()
    {
        var pointX = Random.Range(GameManager.Instance.HiveMovementXMinimum, GameManager.Instance.HiveMovementXMaximum);
        var pointY = Random.Range(GameManager.Instance.HiveMovementYMinimum, GameManager.Instance.HiveMovementYMaximum);
        _hivePointToMove.position = new Vector3(pointX, pointY, 0.0f);
        UpdateHiveDirection();
        UpdateDirectionBoolean();
    }

    private void MoveHive()
    {
        transform.Translate(_hiveDirection.normalized * (_currentSpeed * Time.deltaTime));
        _hive.AnimatorComponent.SetBool("IsMoving", true);
    }

    private void UpdateDirectionBoolean()
    {
        if (_hiveDirection.x > 0.0f)
        {
            _hive.AnimatorComponent.SetBool("IsRight", true);
        }
        else if (_hiveDirection.x < 0.0f)
        {
            _hive.AnimatorComponent.SetBool("IsRight", false);
        }
    }

    private void UpdateHiveDirection()
    {
        _hiveDirection = _hivePointToMove.position - transform.position;
        _hiveDirection.Normalize();
    }
}
