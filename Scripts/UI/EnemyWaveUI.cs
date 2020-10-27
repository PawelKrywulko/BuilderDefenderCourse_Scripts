using System;
using UnityEngine;
using TMPro;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager enemyWaveManager;
    
    private TextMeshProUGUI _waveNumberText;
    private TextMeshProUGUI _waveMessageText;
    private RectTransform _enemyWaveSpawnPositionIndicator;
    private RectTransform _enemyClosestPositionIndicator;
    private Camera _mainCamera;

    private float _targetMaxRadius = 9999f;
    private Enemy _targetEnemy;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
        _waveMessageText = transform.Find("waveMessageText").GetComponent<TextMeshProUGUI>();
        _enemyWaveSpawnPositionIndicator = transform.Find("enemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();
        _enemyClosestPositionIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();
    }

    private void Start()
    {
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManagerOnWaveNumberChanged;
        SetWaveNumberText($"Wave {enemyWaveManager.GetWaveNumber()}");
    }

    private void EnemyWaveManagerOnWaveNumberChanged(object sender, EventArgs e)
    {
        SetWaveNumberText($"Wave {enemyWaveManager.GetWaveNumber()}");
    }

    private void Update()
    {
        HandleNextWaveMessage();
        HandleNextSpawnPositionIndicator();
        HandleEnemyClosestPositionIndicator();
    }

    private void HandleNextWaveMessage()
    {
        float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
        if (nextWaveSpawnTimer <= 0)
        {
            SetMessageText("");
        }
        else
        {
            SetMessageText($"Next Wave in {nextWaveSpawnTimer:F1}s");
        }
    }

    private void HandleNextSpawnPositionIndicator()
    {
        var nextSpawnPositionDirection = (enemyWaveManager.GetEnemySpawnPosition() - _mainCamera.transform.position).normalized;
        _enemyWaveSpawnPositionIndicator.anchoredPosition = nextSpawnPositionDirection * 300f;
        _enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0, 0, Utilities.GetAngleDegreesFromVector(nextSpawnPositionDirection));

        float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetEnemySpawnPosition(), _mainCamera.transform.position);
        _enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > _mainCamera.orthographicSize * 1.5f);
    }
    
    private void HandleEnemyClosestPositionIndicator()
    {
        LookForTargets();

        if (_targetEnemy != null)
        {
            var closestEnemyPositionDirection = (_targetEnemy.transform.position - _mainCamera.transform.position).normalized;
            _enemyClosestPositionIndicator.anchoredPosition = closestEnemyPositionDirection * 250f;
            _enemyClosestPositionIndicator.eulerAngles = new Vector3(0, 0, Utilities.GetAngleDegreesFromVector(closestEnemyPositionDirection));

            float distanceToClosestEnemy = Vector3.Distance(_targetEnemy.transform.position, _mainCamera.transform.position);
            _enemyClosestPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > _mainCamera.orthographicSize * 1.5f);
        }
        else
        {
            //No enemies alive
            _enemyClosestPositionIndicator.gameObject.SetActive(false);
        }
    }
    
    private void LookForTargets()
    {
        var collider2dArray = Physics2D.OverlapCircleAll(_mainCamera.transform.position, _targetMaxRadius);
        _targetEnemy = null;
        foreach (var collider2d in collider2dArray)
        {
            var enemy = collider2d.GetComponent<Enemy>();
            if (enemy)
            {
                //It's an enemy!
                if (!_targetEnemy)
                {
                    _targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <
                        Vector3.Distance(transform.position, _targetEnemy.transform.position))
                    {
                        //This is closer
                        _targetEnemy = enemy;
                    }
                }
            }
        }
    }

    private void SetMessageText(string message)
    {
        _waveMessageText.SetText(message);
    }

    private void SetWaveNumberText(string text)
    {
        _waveNumberText.SetText(text);
    }
}
