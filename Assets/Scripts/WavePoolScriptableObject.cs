using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavePoolScriptableObject", menuName = "ScriptableObject/WavePool")]
public class WavePoolScriptableObject : ScriptableObject
{
    public List<WaveEnemiesScriptableObject> WavesPool;
}
