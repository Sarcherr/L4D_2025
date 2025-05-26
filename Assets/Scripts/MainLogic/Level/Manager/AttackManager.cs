using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

/// <summary>
/// 攻击管理器
/// <para>处理PowerManager发送的攻击请求</para>
/// <para>治疗行为也整合在此</para>
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
    /// 获取攻击结果
    /// </summary>
    /// <param name="origin">发起者</param>
    /// <param name="target">目标</param>
    /// <param name="damageRate">技能伤害倍率</param>
    /// <param name="dotChance">效果命中概率</param>
    /// <returns>攻击结果</returns>
    public Attack GetAttack(string origin, string target, float damageRate, float dotChance = 0f)
    {
        RuntimeUnitData originUnitData = ControllerManager.Instance.AllRuntimeUnitData[origin];
        RuntimeUnitData targetUnitData = ControllerManager.Instance.AllRuntimeUnitData[target];

        bool hit = CheckHit(originUnitData.CurrentHitChance, targetUnitData.CurrentDogeChance);
        bool crit = CheckCrit(originUnitData.CurrentCritChance);
        bool resist = CheckResistance(dotChance, targetUnitData.CurrentResistanceChance);

        if(!hit)
        {
            return new Attack
            {
                Origin = origin,
                Target = target,
                IsHit = false,
                IsCrit = false,
                IsResist = true,
                Damage = 0
            };
        }
        else
        {
            // 基础伤害 = 攻击单位攻击力 * (1 + 攻击单位额外攻击力) * 技能伤害倍率 * 受击单位当前减伤率
            int damage = (int)(originUnitData.CurrentAttack * (1 + originUnitData.CurrentExtraAttackRate)
                * damageRate * targetUnitData.CurrentDamageReductionRate);
            if(crit)
            {
                damage = (int)(damage * originUnitData.CurrentCritRate);
            }

            return new Attack
            {
                Origin = origin,
                Target = target,
                IsHit = true,
                IsCrit = crit,
                IsResist = resist,
                Damage = damage
            };
        }
    }
    /// <summary>
    /// 获取治疗结果
    /// </summary>
    /// <param name="origin">发起者</param>
    /// <param name="target">目标</param>
    /// <param name="healRate">技能治疗倍率</param>
    /// <returns>治疗结果</returns>
    public Heal Heal(string origin, string target, float healRate)
    {
        RuntimeUnitData originUnitData = ControllerManager.Instance.AllRuntimeUnitData[origin];

        bool crit = CheckCrit(originUnitData.CurrentCritChance);
        // 基础治疗 = 攻击单位攻击力 * (1 + 攻击单位额外攻击力) * 技能治疗倍率
        int healValue = (int)(originUnitData.CurrentAttack * (1 + originUnitData.CurrentExtraAttackRate)
            * healRate);
        if (crit)
        {
            healValue = (int)(healValue * originUnitData.CurrentCritRate);
        }
        return new Heal
        {
            Origin = origin,
            Target = target,
            IsCrit = crit,
            HealValue = healValue
        };
    }

    /// <summary>
    /// 处理攻击行为
    /// </summary>
    /// <param name="attack"></param>
    public void HandleAttack(Attack attack)
    {
        // todo: UI效果  
        // todo: 受击效果触发  

        // 检测目标是否拥有怠惰(Laze)Ego  
        var egoContainer = ControllerManager.Instance.AllEgoContainers[attack.Target];
        var lazeEgo = egoContainer.UnitEgo.LastOrDefault(e => e.EgoType == "Laze");

        if (!lazeEgo.Equals(default(Ego)))
        {
            // 若拥有将目标更改为Ego赋予者并消耗一点Ego(原目标UnitEgo中最后获得的此类型Ego)触发其效果  
            attack.Target = lazeEgo.HostName;
            egoContainer.RemoveEgo(new List<int> { egoContainer.UnitEgo.IndexOf(lazeEgo) });
            egoContainer.EgoMachine.TriggerEgo(lazeEgo, "Consume");
        }

        // 处理攻击结果
        var targetUnitData = ControllerManager.Instance.AllRuntimeUnitData[attack.Target];
        targetUnitData.CurrentHealth -= attack.Damage;
    }
    /// <summary>
    /// 处理治疗行为
    /// </summary>
    /// <param name="heal"></param>
    public void HandleHeal(Heal heal)
    {
        // todo: UI效果
        // todo: 受治疗效果触发

        // 处理治疗结果
        var targetUnitData = ControllerManager.Instance.AllRuntimeUnitData[heal.Target];
        targetUnitData.CurrentHealth += heal.HealValue;
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

/// <summary>
/// 攻击结果
/// </summary>
public struct Attack
{
    /// <summary>
    /// 发起者名称
    /// </summary>
    public string Origin;
    /// <summary>
    /// 目标名称
    /// </summary>
    public string Target;

    /// <summary>
    /// 是否命中
    /// </summary>
    public bool IsHit;
    /// <summary>
    /// 是否暴击
    /// </summary>
    public bool IsCrit;
    /// <summary>
    /// 是否抵抗
    /// </summary>
    public bool IsResist;
    /// <summary>
    /// 伤害值
    /// </summary>
    public int Damage;
}

/// <summary>
/// 治疗结果
/// </summary>
public struct Heal
{
    /// <summary>
    /// 发起者名称
    /// </summary>
    public string Origin;
    /// <summary>
    /// 目标名称
    /// </summary>
    public string Target;

    /// <summary>
    /// 是否暴击
    /// </summary>
    public bool IsCrit;
    /// <summary>
    /// 治疗值
    /// </summary>
    public int HealValue;
}
