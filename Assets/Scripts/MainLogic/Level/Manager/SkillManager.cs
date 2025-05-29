using System;
using System.Collections.Generic;
using System.Linq;

public class SkillManager : Singleton<SkillManager>
{
    /// <summary>
    /// 技能行为执行者
    /// </summary>
    public SkillExecutor SkillExecutor = new();

    /// <summary>
    /// 处理技能请求
    /// </summary>
    /// <param name="request"></param>
    public void HandleRequest(SkillRequest request)
    {
        SkillExecutor.ExecuteSkill(request);
    }
}

public class SkillExecutor
{
    public Dictionary<string, Action<SkillRequest>> SkillActions = new();

    public SkillExecutor()
    {
        // todo:初始化注册所有技能行为对应的方法，名称索引与SkillRequest.Name一致
        // ps. 方法格式统一为void MethodName(SkillRequest request)
        // 使用反射自动注册所有方法
        var methods =
            GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        foreach (var method in methods)
        {
            if (method.ReturnType == typeof(void) && method.GetParameters().Length == 1 &&
                method.GetParameters()[0].ParameterType == typeof(SkillRequest))
            {
                SkillActions[method.Name] =
                    (Action<SkillRequest>)Delegate.CreateDelegate(typeof(Action<SkillRequest>), this, method);
            }
        }
    }

    public void ExecuteSkill(SkillRequest request)
    {
        if (SkillActions.TryGetValue(request.Name, out var action))
        {
            action.Invoke(request);
        }
    }

    /// <summary>
    /// 德劳拉-我的审判
    /// <para>将自身所有的特殊ego转移到指定人物身上（包括敌人）</para>
    /// </summary>
    /// <param name="request"></param>
    public void Trial(SkillRequest request)
    {
        var originContainer = ControllerManager.Instance.AllEgoContainers[request.Origin];
        var targetContainer = ControllerManager.Instance.AllEgoContainers[request.Target.FirstOrDefault()];

        // 使用Linq获取自身EgoContainer中所有特殊Ego的索引(即EgoType不为"Normal")  
        var specialEgoIDs = originContainer.UnitEgo
            .Select((ego, index) => new { ego, index })
            .Where(x => x.ego.EgoType != "Normal")
            .Select(x => x.index)
            .ToList();

        // 将specialEgos转移到targetContainer  
        if (targetContainer != null)
        {
            var egos = originContainer.RemoveEgo(specialEgoIDs);
            targetContainer.GainEgo(egos);
        }
    }
}
