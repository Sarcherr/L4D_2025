using System.Collections.Generic;
using System;

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
        foreach (var unitData in controller.Units.Values)
        {
            UnitEgoContainers.Add(unitData.Name, new(unitData, this));
        }
        
        //EventCenter.Instance.Subscribe<EgoArgs>("GainEgo", GainEgo);
        //EventCenter.Instance.Subscribe<EgoArgs>("Remove", RemoveEgo);
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
    /// 使目标Ego容器获取Ego
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    /// <param name="sender"></param>
    /// <param name="egoArgs"></param>
    public void GainEgo<TEventArgs>(object sender, TEventArgs egoArgs)
    {
        if(egoArgs is EgoArgs)
        {
            UnitEgoContainers.TryGetValue((egoArgs as EgoArgs).TargetName, out var container);
            if (container != null)
            {
                container.GainEgo((egoArgs as EgoArgs).Ego2Gain);
            }
        }
    }
    /// <summary>
    /// 使目标Ego容器移除Ego
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    /// <param name="sender"></param>
    /// <param name="egoArgs"></param>
    public void RemoveEgo<TEventArgs>(object sender, TEventArgs egoArgs)
    {
        if(egoArgs is EgoArgs)
        {
            UnitEgoContainers.TryGetValue((egoArgs as EgoArgs).TargetName, out var container);
            if (container != null)
            {
                container.RemoveEgo((egoArgs as EgoArgs).Ego2Remove);
            }
        }
    }
    // todo:移除的具体方案待完善

    /// <summary>
    /// 触发Ego特效
    /// </summary>
    /// <param name="ego">对应Ego</param>
    /// <param name="triggerType">触发类型(爆发Burst/失控OutOfControl)</param>
    public void TriggerEgo(Ego ego, string triggerType)
    {
        EgoExecutor.ExecuteEgo(ego, triggerType);
    }
}

/// <summary>
/// Ego特效执行器
/// </summary>
public class EgoExecutor
{
    public Dictionary<string, Action<Ego, string>> EgoActions = new();

    public EgoExecutor()
    {
        // todo:初始化注册所有Ego行为对应的方法
        // ps. 方法格式统一为void MethodName(Ego ego, string triggerType)
    }

    public void ExecuteEgo(Ego ego, string triggerType)
    {
        if (EgoActions.TryGetValue(ego.EgoType, out var action))
        {
            action.Invoke(ego, triggerType);
        }
    }


}

/// <summary>
/// Ego相关操作参数类
/// </summary>
public class EgoArgs : EventArgs
{
    /// <summary>
    /// 目标名称
    /// </summary>
    public string TargetName;
    /// <summary>
    /// 待获取的Ego(无需指定时为null)
    /// </summary>
    public List<Ego> Ego2Gain;
    /// <summary>
    /// 待移除的Ego(ID，无需指定时为null)
    /// </summary>
    public List<int> Ego2Remove;
    /// <summary>
    /// 待移除的Ego数量(无需指定时为0)
    /// </summary>
    public int Ego2RemoveCount;

    public EgoArgs(string targetName, List<Ego> ego2Gain, List<int> ego2Remove, int ego2RemoveCount)
    {
        TargetName = targetName;
        Ego2Gain = ego2Gain;
        Ego2Remove = ego2Remove;
        Ego2RemoveCount = ego2RemoveCount;
    }
}
