using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float shootTimerMax = 0.3f;
    [SerializeField] private float targetMaxRadius = 20f;

    private float _shootTimer;
    private Transform _projectileSpawnPosition;
    private Enemy _targetEnemy;
    private float _lookForTargetTimer;
    private float _lookForTargetTimerMax = 0.2f;

    private void Awake()
    {
        _shootTimer = shootTimerMax;
        _projectileSpawnPosition = transform.Find("projectile spawn position");
    }

    private void Update()
    {
        HandleTargeting();
        HandleShooting();
    }

    private void HandleShooting()
    {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0f)
        {
            _shootTimer = shootTimerMax;
            if (_targetEnemy)
            {
                ArrowProjectile.Create(_projectileSpawnPosition.position, _targetEnemy);
            }
        }
    }

    private void HandleTargeting()
    {
        _lookForTargetTimer -= Time.deltaTime;
        if (_lookForTargetTimer < 0f)
        {
            _lookForTargetTimer = _lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void LookForTargets()
    {
        var collider2dArray = Physics2D.OverlapCircleAll(_projectileSpawnPosition.position, targetMaxRadius);
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
                    if (Vector3.Distance(_projectileSpawnPosition.position, enemy.transform.position) <
                        Vector3.Distance(_projectileSpawnPosition.position, _targetEnemy.transform.position))
                    {
                        //This is closer
                        _targetEnemy = enemy;
                    }
                }
            }
        }
    }
}
