using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    /// <summary>
    /// 储存所有角色信息，索引为名称
    /// </summary>
    public static readonly Dictionary<string, CharacterData> CharacterDataDic = new();
    /// <summary>
    /// 储存所有敌人信息，索引为名称
    /// </summary>
    public static readonly Dictionary<string, EnemyData> EnemyDataDic = new();

    public static void Init()
    {

    }
}
