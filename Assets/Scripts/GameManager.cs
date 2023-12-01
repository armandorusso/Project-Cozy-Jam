using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

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

    [Header("Hive")]
    [SerializeField] private Hive _hiveCharacter;
    public Hive Hive => _hiveCharacter;
    [Range(-10.0f, -3.0f)] public float HiveMovementXMinimum;
    [Range(3.0f, 10.0f)] public float HiveMovementXMaximum;
    [Range(-6.0f, -1.0f)] public float HiveMovementYMinimum;
    [Range(1.0f, 6.0f)] public float HiveMovementYMaximum;
    public int _currentScore;
    public int _highScore;
    
    [Header("Spawners")]
    [SerializeField] private List<Transform> _enemySpawners = new List<Transform>();
    [SerializeField] private int _maximumEnemiesOnScene;
    [SerializeField] private float _spawnTimerMin;
    [SerializeField] private float _spawnTimerMax;
    public int CurrentEnemiesOnScene { get; set; }
    private float _spawnCountdown { get; set; }

    [Header("Props and UI")] [SerializeField]
    private Animator _deathBackgroundAnimator;
    [SerializeField] private SpriteRenderer _frontFenceSprite;
    [SerializeField] private GameObject _canvasGroup;

    [Header("Wave Properties")] 
    [SerializeField] public WavePoolScriptableObject WaveTypes;

    [SerializeField] public int EnemyAdditionModifier;
    [SerializeField] public float SpawnTimerModifier;
    [SerializeField] private TextMeshProUGUI _waveText;

    public int WaveNumber;
    private List<WaveEnemiesScriptableObject> _waveTypes => WaveTypes.WavesPool;
    private WaveEnemiesScriptableObject _waveDifficulty;
    private List<GameObject> _enemiesToSpawn;
    private int _totalEnemiesInWave;
    private int _totalEnemiesKilledInWave;
    
    public Animator DeathBackgroundAnimator => _deathBackgroundAnimator;

    private bool _hasGameStarted;
    public bool HasGameStarted => _hasGameStarted;

    public static Action<int> ScoreIncrementedAction;
    public static Action<bool> StartWaveAnnouncementAnimationAction;

    void Start()
    {
        _spawnCountdown = SetRandomSpawnTime();
        StartCoroutine(StartGame());

        EnemyHurt.EnemyDeathAction += OnEnemyKilled;
        
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        ScoreIncrementedAction?.Invoke(_currentScore);

        _enemiesToSpawn = new List<GameObject>();
        SetUpWaveDifficulty();
    }

    void Update()
    {
        if (!_hasGameStarted || _hiveCharacter.HiveHurt.CurrentHealth <= 0) return;
        
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

    private IEnumerator StartGame()
    {
        _waveText.text = $"Wave: {WaveNumber}";
        StartWaveAnnouncementAnimationAction?.Invoke(true);
        yield return new WaitForSeconds(3.87f);
        _hasGameStarted = true;
        foreach (var player in _players)
        {
            player.AnimatorComponent.SetTrigger("StartGame");
            player.PlayerMovement.IsMoving = true;
            player.PlayerMovement.UpdateMovementDirectionSprites();
        }

        _hiveCharacter.AnimatorComponent.SetTrigger("StartGame");
        StartWaveAnnouncementAnimationAction?.Invoke(false);
    }

    private void OnEnemyKilled(GameObject enemy)
    {
        switch (enemy.tag)
        {
            case "Yellow Hornet": _currentScore += 10;
                break;
            case "Black Hornet": _currentScore += 10;
                break;
            case "Big Hornet": _currentScore += 15;
                break;
            case "Tank Hornet": _currentScore += 40;
                break;
            default: _currentScore += 0;
                break;
        }
        
        ScoreIncrementedAction?.Invoke(_currentScore);
        _totalEnemiesKilledInWave++;

        if (_totalEnemiesKilledInWave == _totalEnemiesInWave && CurrentEnemiesOnScene == 0)
        {
            StartCoroutine(SetUpNextWave());
        }
    }

    private IEnumerator SetUpNextWave()
    {
        _totalEnemiesKilledInWave = 0;
        
        _spawnTimerMin -= SpawnTimerModifier;
        _spawnTimerMin = Mathf.Clamp(_spawnTimerMin, 0.5f, _spawnTimerMin);
        
        _spawnTimerMax -= SpawnTimerModifier;
        _spawnTimerMax = Mathf.Clamp(_spawnTimerMax, 1.5f, _spawnTimerMax);
        
        _maximumEnemiesOnScene += EnemyAdditionModifier;
        _spawnTimerMax = Mathf.Clamp(_maximumEnemiesOnScene, 5, 20);
        
        WaveNumber++;

        // Little hack to stop the spawning for a few seconds
        CurrentEnemiesOnScene = _maximumEnemiesOnScene;
        StartWaveAnnouncementAnimationAction?.Invoke(true);
        _waveText.text = $"Wave: ";
        yield return new WaitForSeconds(1.87f);
        _waveText.text = $"Wave: {WaveNumber}";
        yield return new WaitForSeconds(2f);
        StartWaveAnnouncementAnimationAction?.Invoke(false);
        CurrentEnemiesOnScene = 0;
        SetUpWaveDifficulty();
    }

    private float SetRandomSpawnTime()
    {
        return Random.Range(_spawnTimerMin, _spawnTimerMax);
    }
    
    private void SpawnEnemy()
    {
        if (_enemiesToSpawn.Count == 0)
            return;
        
        var spawnerPicker = Random.Range(0, _enemySpawners.Count);
        var enemyPicker = Random.Range(0, _enemiesToSpawn.Count);

        var hornetBase = Instantiate(_enemiesToSpawn[enemyPicker], _enemySpawners[spawnerPicker].position, Quaternion.identity);
        _enemiesToSpawn.RemoveAt(enemyPicker);
        var hornetAnimator = hornetBase.transform.GetChild(0);
        hornetAnimator.SetParent(null);
        hornetBase.GetComponent<EnemyAnimator>().SetHornetAnimator(hornetAnimator);
        
        CurrentEnemiesOnScene++;
    }

    private void SetUpWaveDifficulty()
    {
        switch (WaveNumber)
        {
            case < 5: _waveDifficulty = _waveTypes[Random.Range(0, 2)];
                break;
            case > 6 and <= 10: _waveDifficulty = _waveTypes[Random.Range(2, 4)];
                break;
            case > 10: _waveDifficulty = _waveTypes[Random.Range(3, 5)];
                break;
        }
        
        _enemiesToSpawn = new List<GameObject>(_waveDifficulty.Enemies);
        _totalEnemiesInWave = _enemiesToSpawn.Count;
    }

    public void ResetSpritesForBackground()
    {
        _frontFenceSprite.sortingOrder = 0;
        _canvasGroup.SetActive(false);
    }

    private void OnDestroy()
    {
        EnemyHurt.EnemyDeathAction -= OnEnemyKilled;
    }
}
