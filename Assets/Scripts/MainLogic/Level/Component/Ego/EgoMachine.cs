using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class EgoMachine
{
    /// <summary>
    /// 组件所属控制器
    /// </summary>
    public IController Controller;
    /// <summary>
    /// Ego特效执行器
    /// </summary>
    public EgoExecutor EgoExecutor = new();
    /// <summary>
    /// 所属全部单位的Ego条容器
    /// </summary>
    public Dictionary<string, EgoContainer> UnitEgoContainers = new();

    public EgoMachine(IController controller)
    {
        foreach (var unitData in controller.RuntimeUnits.Values)
        {
            EgoContainer egoContainer = new(unitData, this);
            // 初始化时按单位数据获得一定量普通Ego(仅针对敌人)
            if (unitData.UnitKind == "Enemy")
            {
                egoContainer.OnEgoInit();
            }
            UnitEgoContainers.Add(unitData.Name, egoContainer);
        }
    }

    /// <summary>
    /// 获取指定单位Ego条
    /// </summary>
    /// <param name="name">单位名称</param>
    /// <returns>单位Ego条</returns>
    public List<Ego> GetUnitEgo(string name)
    {
        return UnitEgoContainers[name].UnitEgo;
    }

    /// <summary>
    /// 大回合开始时Ego恢复
    /// </summary>
    public void RecoverEgo()
    {
        foreach (var container in UnitEgoContainers.Values)
        {
            container.OnGeneralEgoRecover();
        }
    }

    /// <summary>
    /// 触发Ego特效
    /// <para>爆发Burst/失控OutOfControl/消耗Consume</para>
    /// </summary>
    /// <param name="egoList">对应Ego列表</param>
    /// <param name="triggerType">触发类型(爆发Burst/失控OutOfControl/消耗Consume)</param>
    /// <param name="belongName">所属单位名称</param>
    public void TriggerEgo(List<Ego> egoList, string triggerType, string belongName)
    {
        // 按EgoType分组  
        var groupedEgos = egoList.GroupBy(ego => ego.EgoType)
                                 .ToDictionary(group => group.Key, group => group.ToList());

        // 对每个EgoType的List调用Execute方法  
        foreach (var egoGroup in groupedEgos)
        {
            EgoExecutor.ExecuteEgo(egoGroup.Value, triggerType, belongName);
        }
    }
}

// Attention: 记得修改
// 增添触发Ego效果的逻辑，不是逐个触发，而是通过EgoExecutor统一处理单次生效内所有Ego的触发逻辑

/// <summary>
/// Ego特效执行器
/// </summary>
public class EgoExecutor
{
    public Dictionary<string, Action<List<Ego>, string, string>> EgoActions = new();

    public EgoExecutor()
    {
        // todo:初始化注册所有Ego行为对应的方法
        // ps. 方法格式统一为void MethodName(Ego ego, string triggerType)
        //      方法名称为"EgoType" + "Method"
        // 使用反射自动注册所有方法
        var methods = 
            GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        foreach (var method in methods)
        {
            if (method.ReturnType == typeof(void) && method.GetParameters().Length == 3 &&
                method.GetParameters()[0].ParameterType == typeof(List<Ego>) &&
                method.GetParameters()[1].ParameterType == typeof(string) &&
                method.GetParameters()[2].ParameterType == typeof(string))
            {
                EgoActions[method.Name] =
                    (Action<List<Ego>, string, string>)
                    Delegate.CreateDelegate(typeof(Action<List<Ego>, string, string>), this, method);
            }
        }
    }

    public void ExecuteEgo(List<Ego> egoList, string triggerType, string belongName)
    {
        // 字典索引为"EgoType" + "Method"
        string methodName = egoList.FirstOrDefault().EgoType + "Method";
        if (EgoActions.TryGetValue(methodName, out var action))
        {
            action.Invoke(egoList, triggerType, belongName);
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Ego action '{methodName}' has no trigger type '{triggerType}'.");
        }
    }

    /// <summary>
    /// 愤怒Ego
    /// <para>该ego不可被消耗，可以被转移。当持有者触发情感爆发状态时，每点ego提供10%的攻击力加成与5%的暴击率加成</para>
    /// <para>持有者失控时，每点愤怒ego会对人物产生一次等于当前攻击力的伤害，然后消耗自身</para>
    /// </summary>
    /// <param name="egoList"></param>
    /// <param name="triggerType"></param>
    /// <param name="belongName"></param>
    public void AngerMethod(List<Ego> egoList, string triggerType, string belongName)
    {

    }
}