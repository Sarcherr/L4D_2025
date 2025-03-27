using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能组件
/// <para>执行单位一切非伤害性的主动行为</para>
/// </summary>
public class Skillable
{
    /// <summary>
    /// 技能组件所属控制器
    /// </summary>
    public IController Controller;

    public Skillable(IController controller)
    {
        Controller = controller;
    }
}