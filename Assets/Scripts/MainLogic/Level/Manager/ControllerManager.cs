using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : Singleton<ControllerManager>
{
    /// <summary>
    /// 玩家控制器
    /// </summary>
    public IController Player;
    /// <summary>
    /// 敌人控制器
    /// </summary>
    public IController Enemy;
    /// <summary>
    /// 储存全局UnitData索引
    /// </summary>
    public Dictionary<string, RuntimeUnitData> AllUnitData;
    /// <summary>
    /// 储存全局EgoContainer索引
    /// </summary>
    public Dictionary<string, EgoContainer> AllEgoContainers;

    /// <summary>
    /// 注册控制器
    /// </summary>
    /// <param name="controller">待注册控制器</param>
    public void RegisterController(IController controller)
    {
        if (controller is PlayerController)
        {
            Player = controller;
        }
        else if (controller is EnemyController)
        {
            Enemy = controller;
        }

        foreach (var pair in controller.Units)
        {
            AllUnitData.Add(pair.Key, pair.Value);
        }
        foreach (var pair in controller.EgoMachine.UnitEgoContainers)
        {
            AllEgoContainers.Add(pair.Key, pair.Value);
        }
    }
}
