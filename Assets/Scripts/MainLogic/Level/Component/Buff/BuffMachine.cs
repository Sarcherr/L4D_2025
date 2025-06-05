using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Buff机器，用于处理Buff的添加、移除和应用
/// </summary>
public class BuffMachine
{
    public Dictionary<TurnStage, List<Buff>> UnitBuffs;
    public BuffExecuter Executer;

    public BuffMachine()
    {
        UnitBuffs = new Dictionary<TurnStage, List<Buff>>();
        Executer = new BuffExecuter(this);
    }

    /// <summary>
    /// 添加Buff
    /// <para>注意要new实例化新的buff对象</para>
    /// </summary>
    /// <param name="buff"></param>
    public void AddBuff(Buff buff)
    {
        if (!UnitBuffs.ContainsKey(buff.TurnStage))
        {
            UnitBuffs[buff.TurnStage] = new List<Buff>();
        }

        UnitBuffs[buff.TurnStage].Add(buff);
        Executer.ExecuteBuff(buff, "Add");
    }
    /// <summary>
    /// 移除指定名称的Buff
    /// </summary>
    /// <param name="buffName"></param>
    public void RemoveBuff(string buffName)
    {
        foreach (var stage in UnitBuffs.Keys.ToList()) // 使用ToList()避免修改集合时的异常
        {
            var buffs = UnitBuffs[stage];
            var buffToRemove = buffs.Where(b => b.Name == buffName).ToList();
            foreach (var buff in buffToRemove)
            {
                buffs.Remove(buff);
                Executer.ExecuteBuff(buff, "Remove");
            }
        }
    }
    /// <summary>
    /// 更新指定阶段的所有Buff
    /// </summary>
    /// <param name="stage"></param>
    public void UpdateBuffs(TurnStage stage)
    {
        if (UnitBuffs.ContainsKey(stage))
        {
            var buffs = UnitBuffs[stage];
            foreach (var buff in buffs.ToList()) // 使用ToList()避免修改集合时的异常
            {
                Executer.ExecuteBuff(buff, "Update");
                // 更新buff的回合数
                if (buff.TurnCount > 0)
                {
                    buff.TurnCount--;
                    if (buff.TurnCount <= 0)
                    {
                        RemoveBuff(buff.Name); // 回合数为0时移除Buff
                    }
                }
            }
        }
    }

    /// <summary>
    /// 移除所有情感爆发状态下的Buff
    /// </summary>
    public void RemoveBurstBuffs()
    {
        foreach (var stage in UnitBuffs.Keys.ToList()) // 使用ToList()避免修改集合时的异常
        {
            var buffs = UnitBuffs[stage];
            var burstBuffs = buffs.Where(b => b.Type == BuffType.Burst).ToList();
            foreach (var buff in burstBuffs)
            {
                buffs.Remove(buff);
                Executer.ExecuteBuff(buff, "Remove");
            }
        }
    }
    /// <summary>
    /// 移除所有失控状态下的Buff
    /// </summary>
    public void RemoveOutOfControlBuffs()
    {
        foreach (var stage in UnitBuffs.Keys.ToList()) // 使用ToList()避免修改集合时的异常
        {
            var buffs = UnitBuffs[stage];
            var outOfControlBuffs = buffs.Where(b => b.Type == BuffType.OutOfControl).ToList();
            foreach (var buff in outOfControlBuffs)
            {
                buffs.Remove(buff);
                Executer.ExecuteBuff(buff, "Remove");
            }
        }
    }
}

/// <summary>
/// Buff效果执行器
/// </summary>
public class BuffExecuter
{ 
    public BuffMachine BuffMachine { get; set; }

    public Dictionary<string, Action<Buff, string>> BuffActions = new();

    public BuffExecuter(BuffMachine buffMachine)
    {
        BuffMachine = buffMachine;
        // todo:初始化注册所有buff对应的方法，名称索引与Buff.Name一致
        // ps. 方法格式统一为void MethodName(Buff buff, string type)
        // 使用反射自动注册所有方法
        var methods = GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        foreach (var method in methods)
        {
            if (method.ReturnType == typeof(void) && method.GetParameters().Length == 2 &&
                method.GetParameters()[0].ParameterType == typeof(Buff) && 
                method.GetParameters()[1].ParameterType == typeof(string))
            {
                BuffActions[method.Name] =
                    (Action<Buff, string>)Delegate.CreateDelegate(typeof(Action<Buff, string>), this, method);
            }
        }
    }

    /// <summary>
    /// 执行指定的Buff效果
    /// <para>(添加Add/移除Remove/更新Update)</para>
    /// </summary>
    /// <param name="buff">目标buff</param>
    /// <param name="type">执行类型(添加Add/移除Remove/更新Update)</param>
    public void ExecuteBuff(Buff buff, string type)
    {
        if (BuffActions.TryGetValue(buff.Name, out var action))
        {
            action.Invoke(buff, type);
        }
        else
        {
            Debug.LogWarning($"Buff action '{buff.Name}' has no corresponding method.");
        }
    }

    /// <summary>
    /// 愤怒Ego_爆发状态
    /// <para>当持有者触发情感爆发状态时，每点ego提供10%的攻击力加成与5%的暴击率加成</para>
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="type"></param>
    public void Buff_AngerEgo_Burst(Buff buff, string type)
    {
        if (type == "Add")
        {
            // 添加时，增加攻击力和暴击率加成
            var unitData = ControllerManager.Instance.AllRuntimeUnitData[buff.BelongName];
            unitData.CurrentExtraAttackRate += 0.1f * buff.BuffCount;
            unitData.CurrentCritRate += 0.05f * buff.BuffCount;
        }
        else if (type == "Remove")
        {
            // 移除时，恢复原有攻击力和暴击率
            var unitData = ControllerManager.Instance.AllRuntimeUnitData[buff.BelongName];
            unitData.CurrentExtraAttackRate -= 0.1f * buff.BuffCount;
            unitData.CurrentCritRate -= 0.05f * buff.BuffCount;
        }
    }

    /// <summary>
    /// 沉眠_伤害减免
    /// <para>自身减伤率+50%</para>
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="type"></param>
    public void Buff_Sleepy_DamageReduction(Buff buff, string type)
    {
        if (type == "Add")
        {
            // 添加时，增加减伤率
            var unitData = ControllerManager.Instance.AllRuntimeUnitData[buff.BelongName];
            unitData.CurrentDamageReductionRate += 0.5f; // 增加50%的减伤率
        }
        else if (type == "Remove")
        {
            // 移除时，恢复原有减伤率
            var unitData = ControllerManager.Instance.AllRuntimeUnitData[buff.BelongName];
            unitData.CurrentDamageReductionRate -= 0.5f; // 恢复50%的减伤率
        }
    }
    /// <summary>
    /// 醒梦_Ego恢复
    /// <para>自身下回合ego回复量+2</para>
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="type"></param>
    public void Buff_Waking_dream_EgoRecovery(Buff buff, string type)
    {
        if (type == "Add")
        {
            // 添加时，增加下回合ego回复量
            var unitData = ControllerManager.Instance.AllRuntimeUnitData[buff.BelongName];
            unitData.EgoRecoverValue += 2; // 增加2点ego回复量
        }
        else if (type == "Remove")
        {
            // 移除时，恢复原有下回合ego回复量
            var unitData = ControllerManager.Instance.AllRuntimeUnitData[buff.BelongName];
            unitData.EgoRecoverValue -= 2; // 恢复2点ego回复量
        }
    }
}
