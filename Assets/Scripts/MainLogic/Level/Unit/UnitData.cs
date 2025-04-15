using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单位数据结构体，用于存储单位的基础数据
/// </summary>
public class UnitData
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
    /// 能力列表
    /// </summary>
    public List<PowerData> PowerList;

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
    /// 生命值
    /// </summary>
    public int Health;
    /// <summary>
    /// 攻击力
    /// </summary>
    public int Attack;
    /// <summary>
    /// 命中率
    /// </summary>
    public int HitChance;
    /// <summary>
    /// 闪避率
    /// </summary>
    public float DogeChance;
    /// <summary>
    /// 暴击率
    /// </summary>
    public float CritChance;
    /// <summary>
    /// 暴击倍率
    /// </summary>
    public float CritRate;
    /// <summary>
    /// 抵抗率
    /// </summary>
    public float ResistanceRate;
    /// <summary>
    /// 减伤率
    /// </summary>
    public float DamageReductionRate;
}
