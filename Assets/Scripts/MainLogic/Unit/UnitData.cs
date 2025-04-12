using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    /// <summary>
    /// 名称/ID
    /// </summary>
    public string Name;
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
    /// Ego上限
    /// </summary>
    public int EgoLimit;
    /// <summary>
    /// Ego阈值(超过进入情感爆发状态)
    /// </summary>
    public int EgoThreshold;
}
