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
    /// </summary>
    /// <param name="ego">对应Ego</param>
    /// <param name="triggerType">触发类型(爆发Burst/失控OutOfControl/消耗Consume)</param>
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
