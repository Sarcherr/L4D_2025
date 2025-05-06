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
    /// 储存全局RuntimrUnitData索引
    /// </summary>
    public Dictionary<string, RuntimeUnitData> AllRuntimeUnitData = new();
    /// <summary>
    /// 储存全局EgoContainer索引
    /// </summary>
    public Dictionary<string, EgoContainer> AllEgoContainers = new();

    /// <summary>
    /// 注册控制器
    /// </summary>
    /// <param name="controller">待注册控制器</param>
    public void RegisterController(IController controller)
    {
        if (controller.CotrollerKind == "Player")
        {
            Player = controller;
        }
        else if (controller.CotrollerKind == "Enemy")
        {
            Enemy = controller;
        }

        if(controller.RuntimeUnits != null)
        {
            foreach (var pair in controller.RuntimeUnits)
            {
                AllRuntimeUnitData.Add(pair.Key, pair.Value);
            }
        }

        if(controller.EgoMachine.UnitEgoContainers != null)
        {
            foreach (var pair in controller.EgoMachine.UnitEgoContainers)
            {
                AllEgoContainers.Add(pair.Key, pair.Value);
            }
        }
    }

    /// <summary>
    /// 切换当前行动单位
    /// </summary>
    /// <param name="name"></param>
    public void SwitchUnit(string name)
    {
        if (AllRuntimeUnitData[name].UnitKind == "Player")
        {
            Player.SwitchUnit(name);
        }
        else if (AllRuntimeUnitData[name].UnitKind == "Enemy")
        {
            Enemy.SwitchUnit(name);
        }
    }
}
