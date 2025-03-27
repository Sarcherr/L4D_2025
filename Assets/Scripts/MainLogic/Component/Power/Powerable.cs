using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 能力组件
/// <para>接收外部UI输入信息，整合执行所有单位的主动行为</para>
/// <para>实际为Attack与Skill的中转</para>
/// </summary>
public class Powerable
{
    /// <summary>
    /// 组件所属控制器
    /// </summary>
    public IController Controller;

    public Powerable(IController controller)
    {
        Controller = controller;
    }

    public void GeneratePower(string origin, string power)
    {

    }
}
