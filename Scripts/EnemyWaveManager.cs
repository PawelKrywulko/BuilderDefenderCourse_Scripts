using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveManager : MonoBehaviour
{
    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave
    }

    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;
    [SerializeField] private float nextWaveSpawnTimer = 10f;
    [SerializeField] private float nextEnemySpawnTimer = 0.15f;
    
    public static EnemyWaveManager Instance { get; private set; }
    public event EventHandler OnWaveNumberChanged;
    
    private int _waveNumber;
    private State _state;
    private Vector3 _enemySpawnPosition;
    private int _enemiesToSpawnCount;
    private float _timeToSpawnNextWave;
    private float _timeToSpawnNextEnemy;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _state = State.WaitingToSpawnNextWave;
        _enemySpawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = _enemySpawnPosition;
        _timeToSpawnNextWave = 3f;
        _timeToSpawnNextEnemy = nextEnemySpawnTimer;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.WaitingToSpawnNextWave:
                SpawnWave();
                break;
            case State.SpawningWave:
                SpawnEnemies();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SpawnEnemies()
    {
        _timeToSpawnNextEnemy -= Time.deltaTime;
        if (_enemiesToSpawnCount > 0 && _timeToSpawnNextEnemy < 0f)
        {
            _timeToSpawnNextEnemy = Random.Range(0f, 0.2f);
            Enemy.Create(_enemySpawnPosition + Utilities.GetRandomDirection() * Random.Range(0, 10f));
            _enemiesToSpawnCount--;

            if (_enemiesToSpawnCount <= 0)
            {
                _state = State.WaitingToSpawnNextWave;
                _enemySpawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
                nextWaveSpawnPositionTransform.position = _enemySpawnPosition;
                _timeToSpawnNextWave = nextWaveSpawnTimer;
            }
        }
    }

    private void SpawnWave()
    {
        _timeToSpawnNextWave -= Time.deltaTime;
        if (_timeToSpawnNextWave< 0f)
        {
            _enemiesToSpawnCount = 3 + 2 * _waveNumber;
            _state = State.SpawningWave;
            _waveNumber++;
            OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetWaveNumber()
    {
        return _waveNumber;
    }

    public float GetNextWaveSpawnTimer()
    {
        return _timeToSpawnNextWave;
    }

    public Vector3 GetEnemySpawnPosition()
    {
        return _enemySpawnPosition;
    }
}
