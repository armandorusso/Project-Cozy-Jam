using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveEnemiesScriptableObject", menuName = "ScriptableObject/WaveEnemies")]
public class WaveEnemiesScriptableObject : ScriptableObject
{
    public List<GameObject> Enemies;
}
