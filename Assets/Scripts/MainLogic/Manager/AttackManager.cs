using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击管理器
/// <para>处理Attackable组件发送的攻击请求</para>
/// </summary>
public class AttackManager : Singleton<AttackManager>
{
    /// <summary>
    /// 随机数生成器
    /// </summary>
    public System.Random Random = new();

    /// <summary>
    /// 检查是否命中
    /// </summary>
    /// <param name="originHitChance">发动攻击单位命中率</param>
    /// <param name="targetDogeChance">受击单位闪避率</param>
    /// <returns></returns>
    public bool CheckHit(float originHitChance, float targetDogeChance)
    {
        return Random.NextDouble() < (originHitChance * (1 - targetDogeChance));
    }

    /// <summary>
    /// 检查是否暴击
    /// </summary>
    /// <param name="originCritChance">发动攻击单位暴击率</param>
    /// <returns></returns>
    public bool CheckCrit(float originCritChance)
    {
        return Random.NextDouble() < originCritChance;
    }

    /// <summary>
    /// 检查是否抵抗
    /// </summary>
    /// <param name="originDotRate">发动攻击单位施加dot概率</param>
    /// <param name="targetResistanceRate">受击单位抵抗率</param>
    /// <returns></returns>
    public bool CheckResistance(float originDotRate, float targetResistanceRate)
    {
        return Random.NextDouble() < (originDotRate * (1 - targetResistanceRate));
    }
}
