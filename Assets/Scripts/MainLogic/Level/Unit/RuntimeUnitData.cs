using System.Collections;
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
    public int CurrentHealth;
    /// <summary>
    /// 当前攻击力
    /// </summary>
    public int CurrentAttack;
    /// <summary>
    /// 当前命中率
    /// </summary>
    public float CurrentHitChance;
    /// <summary>
    /// 当前闪避率
    /// </summary>
    public float CurrentDogeChance;
    /// <summary>
    /// 当前暴击率
    /// </summary>
    public float CurrentCritChance;
    /// <summary>
    /// 当前暴击倍率
    /// </summary>
    public float CurrentCritRate;
    /// <summary>
    /// 当前抵抗率
    /// </summary>
    public float CurrentResistanceRate;
    /// <summary>
    /// 当前减伤率
    /// </summary>
    public float CurrentDamageReductionRate;

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
        CurrentResistanceRate = unitData.ResistanceRate;
        CurrentDamageReductionRate = unitData.DamageReductionRate;
        IsBurst = false;
        IsOutOfControl = false;
    }
}
