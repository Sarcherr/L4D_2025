using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public void InitBattle(List<string> playerUnits, List<string> enemyUnits)
    {
        // 初始化战斗
        // 1. 初始化单位/控制器
        // 2. 初始化TurnSystem
        // 3. 初始化UI
        // 4. 开始战斗

        // 步骤1：初始化单位/控制器

        // 以string索引从GlobalData.RuntimeUnitDataDic中获取单位数据
        List<RuntimeUnitData> playerUnitData = new List<RuntimeUnitData>();
        if(playerUnits != null )
        {
            foreach (string unitName in playerUnits)
            {
                if (GlobalData.RuntimeUnitDataDic.TryGetValue(unitName, out RuntimeUnitData unitData))
                {
                    playerUnitData.Add(unitData);
                    Debug.Log($"Unit {unitName} found in RuntimeUnitDataDic");
                }
                else
                {
                    Debug.LogError($"Unit {unitName} not found in RuntimeUnitDataDic");
                }
            }
        }
        Controller player = new Controller("Player", playerUnitData);

        List<RuntimeUnitData> enemyUnitData = new List<RuntimeUnitData>();
        if(enemyUnits != null)
        {
            foreach (string unitName in enemyUnits)
            {
                if (GlobalData.UnitDataDic.TryGetValue(unitName, out UnitData unitData))
                {
                    RuntimeUnitData runtimeUnitData = new RuntimeUnitData();
                    runtimeUnitData.CopyData(unitData);
                    enemyUnitData.Add(runtimeUnitData);
                }
                else
                {
                    Debug.LogError($"Unit {unitName} not found in RuntimeUnitDataDic");
                }
            }
        }
        Controller enemy = new Controller("Enemy", enemyUnitData);

        ControllerManager.Instance.RegisterController(player);
        ControllerManager.Instance.RegisterController(enemy);

        // 步骤2：初始化TurnSystem
        TurnManager.Instance.RefreshQueue();

        // 步骤3：初始化UI
        // todo
        UIManager.Instance.RefreshSkillButton();

        // 步骤4：开始战斗
        // todo
    }
}
