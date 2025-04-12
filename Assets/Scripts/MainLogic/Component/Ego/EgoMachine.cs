using System.Collections.Generic;
using System;

public class EgoMachine
{
    /// <summary>
    /// 所属全部单位的Ego条容器
    /// </summary>
    public Dictionary<string, EgoContainer> UnitEgoContainers = new();

    public EgoMachine(List<UnitData> unitDatas)
    {
        foreach (var unitData in unitDatas)
        {
            UnitEgoContainers.Add(unitData.Name, new(unitData));
        }
        
        EventCenter.Instance.Subscribe<EgoArgs>("gainEgo", GainEgo);
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
