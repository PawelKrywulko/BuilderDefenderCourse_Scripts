using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    private Transform _barTransform;
    private Transform _separatorContainer;
    private Transform _separatorTemplate;

    private void Awake()
    {
        _barTransform = transform.Find("bar");
        ConstructHealthSeparators();
    }

    private void ConstructHealthSeparators()
    {
        _separatorContainer = transform.Find("separatorContainer");
        _separatorTemplate = _separatorContainer.Find("separatorTemplate");
        _separatorTemplate.gameObject.SetActive(false);

        int healthAmountPerSeparator = 10;
        float barSize = 3f;
        float barOneHealthAmountSize = barSize / healthSystem.GetHealthAmountMax();
        int healthSeparatorCount = Mathf.FloorToInt(healthSystem.GetHealthAmountMax() / healthAmountPerSeparator);
        for (int i = 1; i < healthSeparatorCount; i++)
        {
            Transform separatorTransform = Instantiate(_separatorTemplate, _separatorContainer);
            separatorTransform.gameObject.SetActive(true);
            separatorTransform.localPosition = new Vector3(barOneHealthAmountSize * i * healthAmountPerSeparator, 0, 0);
        }
    }

    private void Start()
    {
        healthSystem.OnDamaged += HealthSystemOnDamaged;
        healthSystem.OnHealed += HealthSystemOnHealed;
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystemOnHealthAmountMaxChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void HealthSystemOnHealed(object sender, EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystemOnDamaged(object sender, EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void UpdateBar()
    {
        _barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(),1,1);
    }

    private void UpdateHealthBarVisible()
    {
        if (healthSystem.IsHealthFull())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
