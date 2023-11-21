using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeeScriptableObject", menuName = "ScriptableObject/Bee")]
public class BeePoolScriptableObject : ScriptableObject
{
    public BeeAlly BeeObj;
    public List<BeeAlly> BeePool;
    private List<BeeAlly> _attackingBees;
    private bool _isAttacking;
    public int TotalNumberOfBees;
    public int TotalBeesSpawned;

    public void InstantiateCongoPool()
    {
        BeePool = new List<BeeAlly>(TotalNumberOfBees);
        _attackingBees = new List<BeeAlly>(TotalNumberOfBees);
        TotalBeesSpawned = 0;
    }
    
    public void SpawnBee(Transform playerPosition)
    {
        if (!_isAttacking && TotalBeesSpawned < TotalNumberOfBees)
        {
            var bee = Instantiate(BeeObj, playerPosition.position, Quaternion.identity);
            bee.Index = TotalBeesSpawned;

            if (TotalBeesSpawned == 0)
            {
                bee.ObjectToFollow = playerPosition;
            }
            else
            {
                bee.ObjectToFollow = BeePool.Find(x => x.Index == TotalBeesSpawned - 1).transform;
                bee.transform.position = bee.ObjectToFollow.position;
            }

            BeePool.Add(bee.GetComponent<BeeAlly>());
            TotalBeesSpawned++;
        }
    }
    
    public void RemoveAttackingBeesFromPool(int startingIndex, Transform playerPosition)
    {
        _isAttacking = true;
        for (var i = startingIndex; i >= 0; i--)
        {
            var bee = BeePool[i];
            BeePool.RemoveAt(i);
            _attackingBees.Add(bee);
        }

        TotalBeesSpawned -= startingIndex + 1;
        MoveRemainingBeesToFront(playerPosition);
    }

    private void MoveRemainingBeesToFront(Transform playerPosition)
    {
        if (BeePool.Count == 0)
            return;
        
        for (var i = 0; i < BeePool.Count; i++)
        {
            var bee = BeePool[i];
            bee.Index = i;
            
            if (i == 0)
            {
                bee.ObjectToFollow = playerPosition;
            }
            else
            {
                bee.ObjectToFollow = BeePool.Find(x => x.Index == i - 1).transform;
                bee.transform.position = bee.ObjectToFollow.position;
            }
        }
    }

    public void MoveAttackingBeesToCongoLine(Transform playerPosition)
    {
        foreach (var bee in _attackingBees)
        {
            var lastBeeIndex = BeePool.Count - 1 <= 0 ? BeePool.Count : BeePool.Count - 1;
            if (lastBeeIndex == 0)
            {
                bee.ObjectToFollow = playerPosition;
                bee.PreviousPosition = playerPosition.position;
            }
            else
            {
                bee.ObjectToFollow = BeePool.Find(x => x.Index == lastBeeIndex).transform;
                bee.PreviousPosition = bee.ObjectToFollow.position;
            }
            
            bee.Index = lastBeeIndex + 1;
            BeePool.Add(bee.GetComponent<BeeAlly>());
        }

        TotalBeesSpawned = BeePool.Count;
        _isAttacking = false;
        _attackingBees.Clear();
    }

    public void RemoveAllBees()
    {
        _isAttacking = false;
        BeePool.Clear();
    }
}
