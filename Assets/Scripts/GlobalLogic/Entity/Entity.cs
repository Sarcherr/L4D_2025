using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : IEntity
{
    /// <summary>
    /// 唯一标识
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 世界坐标
    /// </summary>
    public Vector3 WorldPosition { get; set; }
}
