using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class HiveMovement : MonoBehaviour
{
    private Hive _hive;


    [SerializeField] private Transform _hivePointToMove;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _hiveDetectionRange;
    private Vector3 _hiveDirection;

    // Start is called before the first frame update
    void Start()
    {
        _hive = GetComponent<Hive>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _hivePointToMove.position) >= _hiveDetectionRange)
        {
            MoveHive();
        }
    }

    public void PickPointToMove()
    {
        var pointX = Random.Range(GameManager.Instance.HiveMovementXMinimum, GameManager.Instance.HiveMovementXMaximum);
        var pointY = Random.Range(GameManager.Instance.HiveMovementYMinimum, GameManager.Instance.HiveMovementYMaximum);
        _hivePointToMove.position = new Vector3(pointX, pointY, 0.0f);
        _hiveDirection = _hivePointToMove.position - transform.position;
        _hiveDirection.Normalize();
    }
    
    private void MoveHive()
    {
        transform.Translate(_hiveDirection * (_movementSpeed * Time.deltaTime));
    }
}
