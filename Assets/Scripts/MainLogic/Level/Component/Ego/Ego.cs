using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ego结构体
/// </summary>
public struct Ego
{
    /// <summary>
    /// Ego种类
    /// </summary>
    public string EgoType;
    /// <summary>
    /// 赋予者名称
    /// </summary>
    public string HostName;
    /// <summary>
    /// 能否消耗(常规方式)
    /// </summary>
    public bool CanConsume;
}
