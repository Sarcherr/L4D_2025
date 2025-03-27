using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击组件
/// <para>执行单位一切伤害性的主动行为</para>
/// </summary>
public class Attackable
{
    /// <summary>
    /// 组件所属控制器
    /// </summary>
    public IController Controller;

    public Attackable(IController controller)
    {
        Controller = controller;
    }

    /// <summary>
    /// 发动攻击
    /// </summary>
    public void GenerateAttack()
    {

    }
}

/// <summary>
/// 攻击请求容器
/// </summary>
public struct Attack
{
    /// <summary>
    /// 攻击者
    /// </summary>
    public UnitData Origin;
    /// <summary>
    /// 攻击目标
    /// </summary>
    public UnitData Target;
    /// <summary>
    /// 随机系数
    /// </summary>
    public (float min, float max) RandomCoefficient;
    /// <summary>
    /// Ego消耗
    /// </summary>
    public List<Ego> EgoComsumption;
}
