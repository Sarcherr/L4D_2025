using System.Collections.Generic;

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
    /// 当前行动单位数据(全局)
    /// </summary>
    public RuntimeUnitData CurrentUnit;
    /// <summary>
    /// 当前行动单位所属控制器(全局)
    /// </summary>
    public IController CurrentController;

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

        if (controller.RuntimeUnits != null)
        {
            foreach (var pair in controller.RuntimeUnits)
            {
                AllRuntimeUnitData.Add(pair.Key, pair.Value);
            }
        }

        if (controller.EgoMachine.UnitEgoContainers != null)
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
        var unitData = AllRuntimeUnitData[name];

        if (unitData.UnitKind == "Player")
        {
            Player.SwitchUnit(name);
            CurrentController = Player;
        }
        else if (unitData.UnitKind == "Enemy")
        {
            Enemy.SwitchUnit(name);
            CurrentController = Enemy;
        }

        CurrentUnit = unitData;
    }

    /// <summary>
    /// 大回合开始时Ego恢复
    /// </summary>
    public void RecoverEgo()
    {
        Player.EgoMachine.RecoverEgo();
        Enemy.EgoMachine.RecoverEgo();
    }
}
