using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 运行时单位数据，用于关卡内单位的数据记录
/// </summary>
public class RuntimeUnitData
{
    /// <summary>
    /// 单位种类(玩家Player/敌人Enemy)
    /// </summary>
    public string UnitKind;
    /// <summary>
    /// 名称/ID
    /// </summary>
    public string Name;
    /// <summary>
    /// 名称(中文)
    /// </summary>
    public string Name_CN;

    /// <summary>
    /// Ego上限
    /// </summary>
    public int EgoLimit;
    /// <summary>
    /// Ego阈值(超过进入情感爆发状态)
    /// </summary>
    public int EgoThreshold;
    /// <summary>
    /// Ego初始值(仅针对敌人生效，玩家单位的Ego初始值为0)
    /// </summary>
    public int EgoStartValue;
    /// <summary>
    /// Ego大回合自然恢复值
    /// </summary>
    public int EgoRecoverValue;

    /// <summary>
    /// 能力使用次数记录
    /// </summary>
    public List<(PowerData powerData, int usedCount)> PowerRecord;
    /// <summary>
    /// 是否处于情感爆发状态
    /// </summary>
    public bool IsBurst;
    /// <summary>
    /// 是否处于失控状态
    /// </summary>
    public bool IsOutOfControl;


    /// <summary>
    /// 是否死亡
    /// </summary>
    public bool IsDead;


    public int Health;
    /// <summary>
    /// 总生命值
    /// </summary>

    private int _currentHealth;
    /// <summary>  
    /// 当前生命值  
    /// </summary>  
    public event System.Action<int> OnHealthChanged;
    public int CurrentHealth
    {
        get => Mathf.Max(_currentHealth, 0);
        set
        {
            int oldValue = _currentHealth;
            _currentHealth = Mathf.Max(value, 0);

            if (oldValue != _currentHealth)
            {
                OnHealthChanged?.Invoke(_currentHealth);
            }
            if (_currentHealth <= 0&& IsDead == false)
            {
                // todo: 触发死亡事件的具体实现  
                Debug.Log($"{Name} has died.");
                IsDead = true;
                ControllerManager.Instance?.CheckDeadUnit();
            }
        }
    }
    /// <summary>
    /// 当前攻击力
    /// </summary>
    private int _currentAttack;
    public int CurrentAttack
    {
        get => Mathf.Max(_currentAttack, 0);
        set => _currentAttack = Mathf.Max(value, 0);
    }

    private float _currentHitChance;
    /// <summary>
    /// 当前命中率
    /// </summary>
    public float CurrentHitChance
    {
        get => Mathf.Clamp(_currentHitChance, 0f, 1f);
        set => _currentHitChance = Mathf.Clamp(value, 0f, 1f);
    }

    private float _currentDogeChance;
    /// <summary>
    /// 当前闪避率
    /// </summary>
    public float CurrentDogeChance
    {
        get => Mathf.Clamp(_currentDogeChance, 0f, 1f);
        set => _currentDogeChance = Mathf.Clamp(value, 0f, 1f);
    }

    private float _currentCritChance;
    /// <summary>
    /// 当前暴击率
    /// </summary>
    public float CurrentCritChance
    {
        get => Mathf.Clamp(_currentCritChance, 0f, 1f);
        set => _currentCritChance = Mathf.Clamp(value, 0f, 1f);
    }

    private float _currentCritRate;
    /// <summary>
    /// 当前暴击倍率
    /// </summary>
    public float CurrentCritRate
    {
        get => Mathf.Max(_currentCritRate, 0f);
        set => _currentCritRate = Mathf.Max(value, 0f);
    }

    private float _currentResistanceChance;
    /// <summary>
    /// 当前抵抗率
    /// </summary>
    public float CurrentResistanceChance
    {
        get => Mathf.Clamp(_currentResistanceChance, 0f, 1f);
        set => _currentResistanceChance = Mathf.Clamp(value, 0f, 1f);
    }

    private float _currentDamageReductionRate;
    /// <summary>
    /// 当前减伤率
    /// </summary>
    public float CurrentDamageReductionRate
    {
        get => Mathf.Clamp(_currentDamageReductionRate, 0f, 1f);
        set => _currentDamageReductionRate = Mathf.Clamp(value, 0f, 1f);
    }

    /// <summary>
    /// 当前攻击加成
    /// </summary>
    public float CurrentExtraAttackRate;

    /// <summary>
    /// 复制UnitData数据
    /// </summary>
    /// <param name="unitData"></param>
    public void CopyData(UnitData unitData)
    {
        UnitKind = unitData.UnitKind;
        Name = unitData.Name;
        Name_CN = unitData.Name_CN;
        EgoLimit = unitData.EgoLimit;
        EgoThreshold = unitData.EgoThreshold;
        EgoStartValue = unitData.EgoStartValue;
        EgoRecoverValue = unitData.EgoRecoverValue;
        PowerRecord = new List<(PowerData, int)>();
        foreach (var power in unitData.PowerList)
        {
            PowerRecord.Add((power, 0));
        }
        CurrentHealth = unitData.Health;
        CurrentAttack = unitData.Attack;
        CurrentHitChance = unitData.HitChance;
        CurrentDogeChance = unitData.DogeChance;
        CurrentCritChance = unitData.CritChance;
        CurrentCritRate = unitData.CritRate;
        CurrentResistanceChance = unitData.ResistanceRate;
        CurrentDamageReductionRate = unitData.DamageReductionRate;

        CurrentExtraAttackRate = 0f;
        IsBurst = false;
        IsOutOfControl = false;
        IsDead = false;
    }
}
