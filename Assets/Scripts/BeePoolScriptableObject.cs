using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeeScriptableObject", menuName = "ScriptableObject/Bee")]
public class BeePoolScriptableObject : ScriptableObject
{
    public BeeAlly BeeObj;
    public List<BeeAlly> BeePool;
    public int TotalNumberOfBees;
    public int TotalBeesSpawned;

    public void InstantiateCongoPool()
    {
        BeePool = new List<BeeAlly>(TotalNumberOfBees);
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

    public void RemoveAllBees()
    {
        BeePool.Clear();
    }
}
