using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float targetMaxRadius = 10f;
    
    private Rigidbody2D _rigidbody2d;
    private Transform _targetTransform;
    private Transform _hqTransform;
    private HealthSystem _healthSystem;
    private float _lookForTargetTimer;
    private float _lookForTargetTimerMax = 0.2f;
    
    private void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _hqTransform = BuildingManager.Instance.GetHqBuilding()?.transform;
        _targetTransform = _hqTransform;
        _lookForTargetTimer = Random.Range(0f, _lookForTargetTimerMax);
        
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDied += HealthSystemOnDied;
        _healthSystem.OnDamaged += HealthSystemOnDamaged;
    }

    private void HealthSystemOnDamaged(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(2f, 0.1f);
        ChromaticAberrationEffect.Instance.SetWeight(0.3f);
    }

    private void HealthSystemOnDied(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        CinemachineShake.Instance.ShakeCamera(3f, 0.15f);
        ChromaticAberrationEffect.Instance.SetWeight(0.3f);
        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargeting();
    }

    private void HandleMovement()
    {
        if (_targetTransform)
        {
            Vector3 direction = (_targetTransform.position - transform.position).normalized;
            _rigidbody2d.velocity = direction * moveSpeed;
        }
        else
        {
            _rigidbody2d.velocity = Vector2.zero;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        var building = other.gameObject.GetComponent<Building>();
        if (building)
        {
            //Collided with a building!
            var healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            _healthSystem.Damage(999);
        }
    }

    private void LookForTargets()
    {
        var collider2dArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);
        foreach (var collider2d in collider2dArray)
        {
            var building = collider2d.GetComponent<Building>();
            if (building)
            {
                //It's building!
                if (!_targetTransform)
                {
                    _targetTransform = building.transform;
                }
                else
                {
                    if (Vector3.Distance(transform.position, building.transform.position) < 
                        Vector3.Distance(transform.position, _targetTransform.position))
                    {
                        //This is closer
                        _targetTransform = building.transform;
                    }
                }
            }
        }

        if (!_targetTransform)
        {
            //No targets
            _targetTransform = _hqTransform;
        }
    }

    public static Enemy Create(Vector3 position)
    {
        var enemyTransform = Instantiate(GameAssets.Instance.pfEnemy, position, Quaternion.identity);
        return enemyTransform.GetComponent<Enemy>();
    }
}
