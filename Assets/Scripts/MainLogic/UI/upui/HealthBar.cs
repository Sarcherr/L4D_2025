using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; 
    private string unitName; 
    private RuntimeUnitData unitData; 

    public bool isInit = false;

    public void Init()
    {
        unitName = gameObject.name;
        if (ControllerManager.Instance.AllRuntimeUnitData.TryGetValue(unitName, out RuntimeUnitData data))
        {
            unitData = data;
            unitData.OnHealthChanged += HandleHealthChanged;
            UpdateHealthBar();

            isInit = true;
            return;
        }
        else
        {
            Debug.LogError($"HealthBar: Cannot find unit data for {unitName}");
            isInit = false;
            return;
        }
    }

    void Start()
    {
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
        if (!isInit)
        {
            Init();
        }
        else
        {
            UpdateHealthBar();
        }
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
