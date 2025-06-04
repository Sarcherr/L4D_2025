using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; 
    private string unitName; 
    private RuntimeUnitData unitData; 

    void Start()
    {
        unitName = transform.parent.name;
        if (ControllerManager.Instance.AllRuntimeUnitData.TryGetValue(unitName, out RuntimeUnitData data))
        {
            unitData = data;
            unitData.OnHealthChanged += HandleHealthChanged;
            UpdateHealthBar();
        }
        else
        {
            Debug.LogError($"HealthBar: Cannot find unit data for {unitName}");
        }
    }
    void OnDestroy()
    {
        if (unitData != null)
        {
            unitData.OnHealthChanged -= HandleHealthChanged;
        }
    }
    void Update()
    {
        UpdateHealthBar();
    }
    private void HandleHealthChanged(int newHealth)
    {
        UpdateHealthBar();
    }
    public void UpdateHealthBar()
    {
        if (unitData != null && healthSlider != null)
        {
            float healthPercentage = (float)unitData.CurrentHealth / unitData.Health;
            healthSlider.value = healthPercentage;
        }
    }
    public void OnUnitDataChanged(RuntimeUnitData newUnitData)
    {
        unitData = newUnitData;
        UpdateHealthBar();
    }
}
