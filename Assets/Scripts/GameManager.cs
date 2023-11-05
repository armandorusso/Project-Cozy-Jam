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

    [Header("Players")]
    [SerializeField] private Player[] _players;
    public Player[] Players => _players;

    [Header("Enemies")]
    [SerializeField] private GameObject[] _enemyPrefabs;
    private List<GameObject> _enemies = new List<GameObject>();
    public List<GameObject> Enemies => _enemies;

    [Header("Hive")]
    [SerializeField] private GameObject _hive;
    public GameObject Hive => _hive;
    [Range(-10.0f, -3.0f)] public float HiveMovementXMinimum;
    [Range(3.0f, 10.0f)] public float HiveMovementXMaximum;
    [Range(-6.0f, -1.0f)] public float HiveMovementYMinimum;
    [Range(1.0f, 6.0f)] public float HiveMovementYMaximum;
    public int _currentScore;
    
    [Header("Spawners")]
    [SerializeField] private List<Transform> _enemySpawners = new List<Transform>();
    [SerializeField] private int _maximumEnemiesOnScene;
    [SerializeField] private float _spawnTimerMin;
    [SerializeField] private float _spawnTimerMax;
    public int CurrentEnemiesOnScene;
    private float _spawnCountdown;

    private bool _hasGameStarted;
    public bool HasGameStarted => _hasGameStarted;

    // Start is called before the first frame update
    void Start()
    {
        _spawnCountdown = SetRandomSpawnTime();
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3.0f);
        _hasGameStarted = true;
        foreach (var player in _players)
        {
            player.AnimatorComponent.SetTrigger("StartGame");
            player.PlayerMovement.IsMoving = true;
            player.PlayerMovement.UpdateMovementDirectionSprites();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasGameStarted) return;
        
        if (_spawnCountdown > 0)
        {
            _spawnCountdown -= Time.deltaTime;
        }
        else
        {
            if (CurrentEnemiesOnScene <= _maximumEnemiesOnScene)
            {
                SpawnEnemy();
                _spawnCountdown = SetRandomSpawnTime();
            }
        }
    }

    private float SetRandomSpawnTime()
    {
        return Random.Range(_spawnTimerMin, _spawnTimerMax);
    }
    
    private void SpawnEnemy()
    {
        var spawnerPicker = Random.Range(0, _enemySpawners.Count);
        var enemyPicker = Random.Range(0, _enemyPrefabs.Length);

        var go = Instantiate(_enemyPrefabs[enemyPicker], _enemySpawners[spawnerPicker].position, Quaternion.identity);
        CurrentEnemiesOnScene++;
    }
}
