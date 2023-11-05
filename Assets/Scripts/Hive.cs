using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    [SerializeField] private CircleCollider2D _circleCollider;
    [SerializeField] private HiveMovement _hiveMovement;

    public HiveMovement HiveMovement => _hiveMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!_hiveMovement) _hiveMovement = GetComponent<HiveMovement>();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, _hiveMovement.HiveDetectionRange);
    }
}
