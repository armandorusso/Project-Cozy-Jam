using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeCongoLine : MonoBehaviour
{
    [SerializeField] private BeePoolScriptableObject BeePool;
    [SerializeField] private float SpawnInterval;

    private float _currentTime;
    
    void Start()
    {
        BeePool.InstantiateCongoPool();
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= SpawnInterval)
        {
            BeePool.SpawnBee(transform);
            _currentTime = 0f;
        }
    }

    public void OnDestroy()
    {
        BeePool.RemoveAllBees();
    }
}
