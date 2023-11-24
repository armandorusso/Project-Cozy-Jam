using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeeAlly : MonoBehaviour
{
    [SerializeField] public int Index;
    [SerializeField] public Transform ObjectToFollow;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float AttackSpeed;
    [SerializeField] public float DistanceBetweenBees; // If MovementSpeed and _timeInterval is very high, it will look as though the movement stutters
    [SerializeField] public Animator BeeAC;

    public Vector2 PreviousPosition;
    public Vector2 Direction;
    private float _currentTime;
    private bool _isChargingAttack;
    private float _originalMovementSpeed;

    private void Start()
    {
        PreviousPosition = ObjectToFollow.transform.position;
        _originalMovementSpeed = MovementSpeed;
        BeeAttack.StopAttackingAction += OnAttackFinish;
    }

    void Update()
    {
        FollowBeeOrPlayer();
        
        Debug.DrawRay(transform.position, Direction.normalized * 0.5f, Color.blue);
    }

    private void FollowBeeOrPlayer()
    {
        _currentTime += Time.deltaTime;
        Vector2 currentPosition = transform.position;
        
        if (_isChargingAttack)
        {
            transform.Translate(MovementSpeed * Time.deltaTime, 0f, 0f);
            return;
        }
        
        if(ObjectToFollow != null)
            Direction = ((Vector2) ObjectToFollow.position - currentPosition).normalized;
        
        if (currentPosition != PreviousPosition)
        {
            transform.position = Vector2.Lerp(currentPosition, PreviousPosition,
                MovementSpeed * Time.deltaTime);
        }

        // Acts as a comparison of distance between each bee. This ensures that there is a gap between each bee
        // DistanceBetweenBees acts as a time interval threshold
        // This "delay" will ensure that it has more time to lerp to PreviousPosition 
        if (ObjectToFollow != null && _currentTime >= DistanceBetweenBees)
        {
            PreviousPosition = ObjectToFollow.transform.position;
            _currentTime = 0f;
        }
    }

    public void OnBeeAttack(bool isAttacking)
    {
        StartCoroutine(PlayAttackAnimation(isAttacking));
    }

    private IEnumerator PlayAttackAnimation(bool isAttacking)
    {
        // Pause before attacking
        _isChargingAttack = true;
        Direction = Vector2.zero;
        MovementSpeed = 0f;
        yield return new WaitForSeconds(0.2f);
        
        // Attack!
        BeeAC.SetBool("IsAttacking", isAttacking);
        yield return new WaitForSeconds(0.2f);
        
        // Point towards the enemy and speed up
        Direction = ObjectToFollow.position - transform.position;
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg);
        MovementSpeed = _originalMovementSpeed * AttackSpeed;
        yield return new WaitForSeconds(0.8f);
        
        // Reset animation and values
        _isChargingAttack = false;
        transform.rotation = Quaternion.identity;
        MovementSpeed = _originalMovementSpeed;
    }

    private void OnAttackFinish(bool stopAttack)
    {
        BeeAC.SetBool("IsAttacking", !stopAttack);
        MovementSpeed = _originalMovementSpeed;
    }

    public void MoveTowardsEnemyPosition(Transform enemyPosition)
    {
        ChangeObjectToFollow(enemyPosition);
    }

    private void ChangeObjectToFollow(Transform enemyPosition)
    {
        // Set ObjectToFollow to enemy position and change its speed
        ObjectToFollow = enemyPosition;
        PreviousPosition = transform.position;
        MovementSpeed *= 1.5f;
    }

    public void OnDestroy()
    {
        BeeAttack.StopAttackingAction -= OnAttackFinish;
    }
}
