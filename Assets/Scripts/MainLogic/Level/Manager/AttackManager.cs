using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击管理器
/// <para>处理PowerManager发送的攻击请求</para>
/// </summary>
public class AttackManager : Singleton<AttackManager>
{
    /// <summary>
    /// 随机数生成器
    /// </summary>
    public System.Random Random = new();
    /// <summary>
    /// 攻击行为执行者
    /// </summary>
    public AttackExecutor Executor = new();

    /// <summary>
    /// 处理攻击请求
    /// </summary>
    /// <param name="request"></param>
    public void HandleRequest(AttackRequest request)
    {
        Executor.ExecuteAttack(request);
    }

    /// <summary>
    /// 检查是否命中
    /// </summary>
    /// <param name="originHitChance">发动攻击单位命中率</param>
    /// <param name="targetDogeChance">受击单位闪避率</param>
    /// <returns>是否命中</returns>
    public bool CheckHit(float originHitChance, float targetDogeChance)
    {
        return Random.NextDouble() < (originHitChance * (1 - targetDogeChance));
    }

    /// <summary>
    /// 检查是否暴击
    /// </summary>
    /// <param name="originCritChance">发动攻击单位暴击率</param>
    /// <returns>是否暴击</returns>
    public bool CheckCrit(float originCritChance)
    {
        return Random.NextDouble() < originCritChance;
    }

    /// <summary>
    /// 检查是否抵抗
    /// </summary>
    /// <param name="originDotRate">发动攻击单位施加dot概率</param>
    /// <param name="targetResistanceRate">受击单位抵抗率</param>
    /// <returns>是否抵抗</returns>
    public bool CheckResistance(float originDotRate, float targetResistanceRate)
    {
        return Random.NextDouble() < (originDotRate * (1 - targetResistanceRate));
    }
}

/// <summary>
/// 攻击行为执行者
/// </summary>
public class AttackExecutor
{
    public Dictionary<string, Action<AttackRequest>> AttackActions = new();

    public AttackExecutor()
    {
        // todo:初始化注册所有攻击行为对应的方法
        // ps. 方法格式统一为void MethodName(AttackRequest request)
    }

    /// <summary>
    /// 执行攻击行为
    /// </summary>
    /// <param name="request">当前攻击请求</param>
    public void ExecuteAttack(AttackRequest request)
    {
        if (AttackActions.TryGetValue(request.Name, out var action))
        {
            action.Invoke(request);
        }
    }
}
