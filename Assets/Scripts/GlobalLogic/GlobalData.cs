using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    /// <summary>
    /// 储存所有单位信息，索引为名称
    /// </summary>
    public static readonly Dictionary<string, UnitData> UnitDataDic = new();
    /// <summary>
    /// 储存所有单位的运行时数据，索引为名称
    /// </summary>
    public static readonly Dictionary<string, RuntimeUnitData> RuntimeUnitDataDic = new();
    /// <summary>
    /// 储存所有能力信息，索引为名称(仅技能次数限制)
    /// </summary>
    public static readonly Dictionary<string, PowerData> PowerDataDic = new();

    public static void Init()
    {
        
    }
}
