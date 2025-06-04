using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        Attack attackResult;

        if (!hit)
        {
            attackResult =  new Attack
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
            // 基础伤害 = 攻击单位攻击力 * (1 + 攻击单位额外攻击力) * 技能伤害倍率 * (1 - 受击单位当前减伤率)
            int damage = (int)(originUnitData.CurrentAttack * (1 + originUnitData.CurrentExtraAttackRate)
                * damageRate * (1 - targetUnitData.CurrentDamageReductionRate));
            if (crit)
            {
                damage = (int)(damage * originUnitData.CurrentCritRate);
            }

            attackResult = new Attack
            {
                Origin = origin,
                Target = target,
                IsHit = true,
                IsCrit = crit,
                IsResist = resist,
                Damage = damage
            };
        }

        HandleAttack(attackResult);
        return attackResult;
    }
    /// <summary>
    /// 获取治疗结果
    /// </summary>
    /// <param name="origin">发起者</param>
    /// <param name="target">目标</param>
    /// <param name="healRate">技能治疗倍率</param>
    /// <returns>治疗结果</returns>
    public Heal GetHeal(string origin, string target, float healRate)
    {
        RuntimeUnitData originUnitData = ControllerManager.Instance.AllRuntimeUnitData[origin];

        bool crit = CheckCrit(originUnitData.CurrentCritChance);

        Heal healResult;

        // 基础治疗 = 攻击单位攻击力 * (1 + 攻击单位额外攻击力) * 技能治疗倍率
        int healValue = (int)(originUnitData.CurrentAttack * (1 + originUnitData.CurrentExtraAttackRate)
            * healRate);
        if (crit)
        {
            healValue = (int)(healValue * originUnitData.CurrentCritRate);
        }
        healResult = new Heal
        {
            Origin = origin,
            Target = target,
            IsCrit = crit,
            HealValue = healValue
        };

        HandleHeal(healResult);
        return healResult;
    }

    /// <summary>
    /// 处理攻击行为
    /// </summary>
    /// <param name="attack"></param>
    public void HandleAttack(Attack attack)
    {
        // 检测目标是否拥有怠惰(Laze)Ego  
        var egoContainer = ControllerManager.Instance.AllEgoContainers[attack.Target];
        var lazeEgo = egoContainer.UnitEgo.LastOrDefault(e => e.EgoType == "Laze");

        if (!lazeEgo.Equals(default(Ego)))
        {
            // 若拥有将目标更改为Ego赋予者并消耗一点Ego(原目标UnitEgo中最后获得的此类型Ego)触发其效果  
            attack.Target = lazeEgo.HostName;
            egoContainer.EgoMachine.TriggerEgo(new List<Ego>() { lazeEgo }, "Consume", lazeEgo.HostName);
            egoContainer.RemoveEgo(new List<int> { egoContainer.UnitEgo.IndexOf(lazeEgo) });
        }

        // 处理攻击结果
        var targetUnitData = ControllerManager.Instance.AllRuntimeUnitData[attack.Target];
        targetUnitData.CurrentHealth -= attack.Damage;

        // todo: UI效果  
        // todo: 受击效果触发  
    }
    /// <summary>
    /// 处理治疗行为
    /// </summary>
    /// <param name="heal"></param>
    public void HandleHeal(Heal heal)
    {
        // 处理治疗结果
        var targetUnitData = ControllerManager.Instance.AllRuntimeUnitData[heal.Target];
        targetUnitData.CurrentHealth += heal.HealValue;

        // todo: UI效果
        // todo: 受治疗效果触发
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
        // todo:初始化注册所有攻击行为对应的方法，名称索引与AttackRequest.Name一致
        // ps. 方法格式统一为void MethodName(AttackRequest request)
        // 使用反射自动注册所有方法
        var methods = 
            GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        foreach (var method in methods)
        {
            if (method.ReturnType == typeof(void) && method.GetParameters().Length == 1 &&
                method.GetParameters()[0].ParameterType == typeof(AttackRequest))
            {
                AttackActions[method.Name] = 
                    (Action<AttackRequest>)Delegate.CreateDelegate(typeof(Action<AttackRequest>), this, method);
            }
        }
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

    /// <summary>
    /// 德劳拉-神圣重击
    /// <para>对敌方单体造成一次倍率150%的伤害，命中后获得一点“愤怒ego”若暴击，获得量*3</para>
    /// </summary>
    /// <param name="request"></param>
    public void Holy_strike(AttackRequest request)
    {
        foreach (var target in request.Target)
        {
            Attack attackResult = AttackManager.Instance.GetAttack(request.Origin, target, 1.5f);

            if (attackResult.IsHit)
            {
                // 获取攻击者Ego容器
                var egoContainer = ControllerManager.Instance.AllEgoContainers[request.Origin];
                // 创建愤怒Ego
                Ego angerEgo = new Ego
                {
                    EgoType = "Anger",
                    HostName = request.Origin,
                    CanConsume = false,
                };

                if (attackResult.IsCrit)
                {
                    // 暴击时获得3点愤怒Ego
                    egoContainer.GainEgo(new List<Ego> { angerEgo, angerEgo, angerEgo });
                }
                else
                {
                    // 普通攻击获得1点愤怒Ego
                    egoContainer.GainEgo(new List<Ego> { angerEgo });
                }
            }
        }
    }

    /// <summary>
    /// 德劳拉-一颗葡萄
    /// <para>选择自身一点带有特殊效果的ego将其消耗，回复自身一次倍率50%的血量（可以暴击）</para>
    /// </summary>
    /// <param name="request"></param>
    public void Eat_grape(AttackRequest request)
    {
        Heal healResult = AttackManager.Instance.GetHeal(request.Origin, request.Origin, 0.5f);
    }

    /// <summary>
    /// 德劳拉-未竟誓言
    /// <para>对随机两名敌人造成一次100%倍率的伤害，每命中一个敌人就将自身最后获得的1点ego上附加愤怒ego（依次往前附加）</para>
    /// </summary>
    public void Oaths(AttackRequest request)
    {
        // 目标直接从ControllerManager获取
        // 随机取两名目标，若少于两名则取全部
        var targets = ControllerManager.Instance.AllRuntimeUnitData.Values
            .Where(u => u.UnitKind == "Enemy" && u.Name != request.Origin)
            .OrderBy(_ => AttackManager.Instance.Random.Next())
            .Take(2)
            .Select(u => u.Name)
            .ToList();

        int hitNum = 0;
        foreach(var target in targets)
        {
            Attack attackResult = AttackManager.Instance.GetAttack(request.Origin, target, 1f);
            if (attackResult.IsHit)
            {
                hitNum++;
            }
        }

        if(hitNum > 0)
        {
            // 获取攻击者Ego容器
            var egoContainer = ControllerManager.Instance.AllEgoContainers[request.Origin];
            // 创建愤怒Ego
            Ego angerEgo = new Ego
            {
                EgoType = "Anger",
                HostName = request.Origin,
                CanConsume = false,
            };
            // 附加愤怒Ego
            egoContainer.AttachEgo(hitNum, true, angerEgo, out _);
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
