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
    /// 当前生命值  
    /// </summary>  
    public int CurrentHealth
    {
        get => Mathf.Max(CurrentHealth, 0);
        set
        {
            CurrentHealth = Mathf.Max(value, 0);
            if (CurrentHealth <= 0)
            {
                // todo: 触发死亡事件的具体实现  
            }
        }
    }
    /// <summary>
    /// 当前攻击力
    /// </summary>
    public int CurrentAttack
    {         
        // 数值不小于0
        get => Mathf.Max(CurrentAttack, 0);
        set => CurrentAttack = Mathf.Max(value, 0);
    }
    /// <summary>
    /// 当前命中率
    /// </summary>
    public float CurrentHitChance
    {
        // 范围限制在0到1之间
        get => Mathf.Clamp(CurrentHitChance, 0f, 1f);
        set => CurrentHitChance = Mathf.Clamp(value, 0f, 1f);
    }
    /// <summary>
    /// 当前闪避率
    /// </summary>
    public float CurrentDogeChance
    {
        // 范围限制在0到1之间
        get => Mathf.Clamp(CurrentDogeChance, 0f, 1f);
        set => CurrentDogeChance = Mathf.Clamp(value, 0f, 1f);
    }

    /// <summary>
    /// 当前暴击率
    /// </summary>
    public float CurrentCritChance
    {
        // 范围限制在0到1之间
        get => Mathf.Clamp(CurrentCritChance, 0f, 1f);
        set => CurrentCritChance = Mathf.Clamp(value, 0f, 1f);
    }
    /// <summary>
    /// 当前暴击倍率
    /// </summary>
    public float CurrentCritRate
    {
        // 数值不小于0
        get => Mathf.Max(CurrentCritRate, 0f);
        set => CurrentCritRate = Mathf.Max(value, 0f);
    }
    /// <summary>
    /// 当前抵抗率
    /// </summary>
    public float CurrentResistanceChance
    {
        // 范围限制在0到1之间
        get => Mathf.Clamp(CurrentResistanceChance, 0f, 1f);
        set => CurrentResistanceChance = Mathf.Clamp(value, 0f, 1f);
    }
    /// <summary>
    /// 当前减伤率
    /// </summary>
    public float CurrentDamageReductionRate
    {
        // 范围限制在0到1之间
        get => Mathf.Clamp(CurrentDamageReductionRate, 0f, 1f);
        set => CurrentDamageReductionRate = Mathf.Clamp(value, 0f, 1f);
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
    }
}
