using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeCongoLine : MonoBehaviour
{
    [SerializeField] private BeePoolScriptableObject BeePool;
    
    
    void Start()
    {
        BeePool.SpawnBees(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
