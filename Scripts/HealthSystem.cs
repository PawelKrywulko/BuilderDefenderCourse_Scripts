using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int healthAmountMax;
    
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDied;
    
    private int _healthAmount;

    private void Awake()
    {
        _healthAmount = healthAmountMax;
    }

    public void Damage(int damageAmount)
    {
        _healthAmount -= damageAmount;
        _healthAmount = Mathf.Clamp(_healthAmount, 0, healthAmountMax);
        
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (IsDead())
        {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsDead()
    {
        return _healthAmount == 0;
    }

    public bool IsHealthFull()
    {
        return _healthAmount == healthAmountMax;
    }

    public int GetHealthAmount()
    {
        return _healthAmount;
    }
    
    public int GetHealthAmountMax()
    {
        return healthAmountMax;
    }

    public float GetHealthAmountNormalized()
    {
        return (float)_healthAmount / healthAmountMax;
    }

    public void SetHealthAmountMax(int healthMax, bool updateHealthAmount = false)
    {
        healthAmountMax = healthMax;
        if (updateHealthAmount)
        {
            _healthAmount = healthMax;
        }
    }

    private void Heal(int healAmount)
    {
        _healthAmount += healAmount;
        _healthAmount = Mathf.Clamp(_healthAmount, 0, healthAmountMax);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void HealFull()
    {
        Heal(healthAmountMax);
    }
}
