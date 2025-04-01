using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 能力管理器
/// </summary>
public class PowerManager : Singleton<PowerManager>
{


    protected override void Init()
    {

    }

    /// <summary>
    /// 处理能力请求
    /// </summary>
    /// <param name="powerData"></param>
    public void HandleRequest(PowerRequest powerData)
    {
        // todo:向UI发送请求
    }

    /// <summary>
    /// 接收UI输入结果，生成能力
    /// </summary>
    public void GeneratePower()
    {
        // todo:生成能力
        // todo:生成攻击与技能请求，发送给AttackManager与SkillManager
    }
}

/// <summary>
/// 能力请求容器
/// </summary>
public struct PowerRequest
{
    /// <summary>
    /// 能力名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 发起者名称
    /// </summary>
    public string Origin;
    /// <summary>
    /// 单位种类
    /// </summary>
    public string UnitKind;
}

/// <summary>
/// 攻击请求容器
/// </summary>
public struct AttackRequest
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 攻击者
    /// </summary>
    public string Origin;
    /// <summary>
    /// 攻击目标
    /// </summary>
    public string Target;
    /// <summary>
    /// 随机系数
    /// </summary>
    public (float min, float max) RandomCoefficient;
    /// <summary>
    /// Ego消耗
    /// </summary>
    public List<Ego> EgoComsumption;
}

public struct SkillRequest
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 技能发起者
    /// </summary>
    public string Origin;
    /// <summary>
    /// 技能目标
    /// </summary>
    public string Target;
    /// <summary>
    /// 随机系数
    /// </summary>
    public (float min, float max) RandomCoefficient;
    /// <summary>
    /// Ego消耗
    /// </summary>
    public List<Ego> EgoComsumption;
}