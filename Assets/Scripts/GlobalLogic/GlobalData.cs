using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    /// <summary>
    /// 储存所有角色信息，索引为名称
    /// </summary>
    public static readonly Dictionary<string, UnitData> CharacterDataDic = new();

    public static void Init()
    {

    }
}
