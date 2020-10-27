using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float moveSpeed = 20f;
    
    private Enemy _targetEnemy;
    private Vector3 _lastDirection;
    private float _timeToDie = 2f;

    private void Update()
    {
        Vector3 direction;
        if (_targetEnemy)
        {
            direction = (_targetEnemy.transform.position - transform.position).normalized;
            _lastDirection = direction;
        }
        else
        {
            direction = _lastDirection;
        }
        
        transform.position += direction * (Time.deltaTime * moveSpeed);
        transform.eulerAngles = new Vector3(0, 0, Utilities.GetAngleDegreesFromVector(direction));

        _timeToDie -= Time.deltaTime;
        if (_timeToDie < 0f)
        {
            Destroy(gameObject);
        }
    }

    private void SetTarget(Enemy enemy)
    {
        _targetEnemy = enemy;
    }
    
    public static ArrowProjectile Create(Vector3 position, Enemy enemy)
    {
        var pfArrowProjectile = GameAssets.Instance.pfArrowProjectile;
        var arrowTransform = Instantiate(pfArrowProjectile, position, Quaternion.identity);
        var arrowProjectile = arrowTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);
        return arrowProjectile;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            //Hit an enemy!
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);
            Destroy(gameObject);
        }
    }
}
