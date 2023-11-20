using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeeScriptableObject", menuName = "ScriptableObject/Bee")]
public class BeePoolScriptableObject : ScriptableObject
{
    public BeeAlly BeeObj;
    public List<BeeAlly> BeePool;
    private List<BeeAlly> BeesToDeallocate;
    public int TotalNumberOfBees;
    public int TotalBeesSpawned;

    public void InstantiateCongoPool()
    {
        BeePool = new List<BeeAlly>(TotalNumberOfBees);
        BeesToDeallocate = new List<BeeAlly>(TotalNumberOfBees);
        TotalBeesSpawned = 0;
    }
    
    public void SpawnBee(Transform playerPosition)
    {
        if (TotalBeesSpawned < TotalNumberOfBees)
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
    
    public void RemoveAttackingBeesFromPool(int startingIndex, Transform playerPosition, Transform[] BorderLocations)
    {
        for (int i = startingIndex; i >= 0; i--)
        {
            var bee = BeePool[i];
            BeePool.RemoveAt(i);
            BeesToDeallocate.Add(bee);
        }

        TotalBeesSpawned -= startingIndex + 1;
        MoveRemainingBeesToFront(playerPosition, BorderLocations);
    }

    private void MoveRemainingBeesToFront(Transform playerPosition, Transform[] BorderLocations)
    {
        if (BeePool.Count == 0)
            return;
        
        for (int i = 0; i < BeePool.Count; i++)
        {
            var bee = BeePool[i];
            if (i == 0)
            {
                bee.ObjectToFollow = playerPosition;
                bee.Index = i;
            }
            else
            {
                bee.ObjectToFollow = BeePool.Find(x => x.Index == i - 1).transform;
                bee.Index = i;
                bee.transform.position = bee.ObjectToFollow.position;
            }
        }
    }

    public void MoveAttackingBeesToLocation(Transform[] BorderLocations)
    {
        foreach (var bee in BeesToDeallocate)
        {
            bee.MoveBackToCongoLine(BorderLocations);
        }
    }

    public void RemoveAllBees()
    {
        BeePool.Clear();
    }
}
