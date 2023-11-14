using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeeScriptableObject", menuName = "ScriptableObject/Bee")]
public class BeePoolScriptableObject : ScriptableObject
{
    public Bee BeeObj;
    public List<Bee> BeePool;
    public int TotalNumberOfBees;

    public void SpawnBees(Transform playerPosition)
    {
        BeePool = new List<Bee>(TotalNumberOfBees);
        for (int i = 0; i < TotalNumberOfBees; i++)
        {
            var bee = Instantiate(BeeObj, playerPosition.position, Quaternion.identity);
            bee.Index = i;

            if (i == 0)
            {
                bee.ObjectToFollow = playerPosition;
            }
            else
            {
                bee.ObjectToFollow = BeePool.Find(x => x.Index == i - 1).transform;
            }

            BeePool.Add(bee.GetComponent<Bee>());
        }
    }
}
