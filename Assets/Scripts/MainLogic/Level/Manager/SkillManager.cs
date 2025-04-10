using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }
}

public class SkillExecutor
{
    public Dictionary<string, Action<SkillRequest>> SkillActions = new();

    public SkillExecutor()
    {
        // todo:初始化注册所有技能行为对应的方法
        // ps. 方法格式统一为void MethodName(SkillRequest request)
    }

    public void ExecuteSkill(SkillRequest request)
    {
        if (SkillActions.TryGetValue(request.Name, out var action))
        {
            action.Invoke(request);
        }
    }
}
