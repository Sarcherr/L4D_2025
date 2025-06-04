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
    /// 全局Buff机器，用于处理Buff的添加、移除和应用
    /// </summary>
    public BuffMachine BuffMachine = new();
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
    /// 刷新控制器状态
    /// </summary>
    public void RefreshControllers()
    {
        Player = null;
        Enemy = null;
        AllRuntimeUnitData.Clear();
        AllEgoContainers.Clear();
        CurrentUnit = null;
        CurrentController = null;
    }
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
    /// 检查单位是否死亡
    /// </summary>
    public void CheckDeadUnit()
    {
        foreach (var pair in AllRuntimeUnitData)
        {
            if (pair.Value.IsDead == true)
            {
                if (TurnManager.Instance.CurrentTurn.Name == pair.Key)
                {
                    // 如果当前回合单位死亡，切换到下一个单位
                    TurnManager.Instance.NextTurn();
                }
                // todo: 处理单位死亡逻辑(移除出回合序列等)
            }
        }
    }

    /// <summary>
    /// 切换当前行动单位
    /// </summary>
    /// <param name="name"></param>
    public void SwitchUnit(string name)
    {
        if (name != "End")
        {
            // 结束当前单位的回合
            OnTurnEnd();

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

            // 开始新单位的回合
            OnTurnStart();
        }
    }

    /// <summary>
    /// 大回合开始时Ego恢复
    /// </summary>
    public void RecoverEgo()
    {
        Player.RecoverEgo();
        Enemy.RecoverEgo();
    }
    /// <summary>
    /// 每个单位的回合开始时调用
    /// </summary>
    public void OnTurnStart()
    {
        CurrentController?.OnTurnStart();
    }
    /// <summary>
    /// 每个单位的回合结束时调用
    /// </summary>
    public void OnTurnEnd()
    {
        CurrentController?.OnTurnEnd();
    }
}
