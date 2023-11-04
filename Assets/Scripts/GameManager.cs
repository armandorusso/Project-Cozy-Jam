using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance 
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    [SerializeField] private GameObject[] _players;
    public GameObject[] Players => _players;
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private List<Transform> _enemySpawners = new List<Transform>();
    private List<GameObject> _enemies = new List<GameObject>();
    public List<GameObject> Enemies => _enemies;
    [SerializeField] private GameObject _hive;
    public GameObject Hive => _hive;
    public int _currentScore;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
