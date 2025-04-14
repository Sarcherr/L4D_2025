using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PowerData
{
    /// <summary>
    /// 所属单位名称
    /// </summary>
    public string belongName;
    /// <summary>
    /// 能力名称
    /// </summary>
    public string name;
    /// <summary>
    /// 使用次数上限(0为无上限)
    /// </summary>
    public int limit;
    ///// <summary>
    ///// 随机系数最小值
    ///// </summary>
    //public float minRandomFactor;
    ///// <summary>
    ///// 随机系数最大值
    ///// </summary>
    //public float maxRandomFactor;
}