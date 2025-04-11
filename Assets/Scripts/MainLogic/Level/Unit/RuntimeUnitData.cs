using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 运行时单位数据，用于关卡内单位的数据记录
/// </summary>
public struct RuntimeUnitData
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
    /// Ego上限
    /// </summary>
    public int EgoLimit;
    /// <summary>
    /// Ego阈值(超过进入情感爆发状态)
    /// </summary>
    public int EgoThreshold;

    /// <summary>
    /// 能力使用次数记录
    /// </summary>
    public List<(PowerData, int)> PowerRecord;
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
    public int CurrentHitChance;
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
}
