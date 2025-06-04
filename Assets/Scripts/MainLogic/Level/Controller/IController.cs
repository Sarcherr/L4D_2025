using System.Collections.Generic;

public interface IController
{
    /// <summary>
    /// 控制器类型(Player/Enemy)
    /// </summary>
    public string CotrollerKind { get; set; }
    /// <summary>
    /// 出战单位
    /// </summary>
    public Dictionary<string, RuntimeUnitData> RuntimeUnits { get; set; }
    /// <summary>
    /// 当前行动单位
    /// </summary>
    public string CurrentUnit { get; set; }
    /// <summary>
    /// 能力组件
    /// </summary>
    public Powerable Powerable { get; set; }
    /// <summary>
    /// Ego组件
    /// </summary>
    public EgoMachine EgoMachine { get; set; }
    public int ActionPoint { get; set; }
    /// <summary>
    /// 切换当前行动单位
    /// </summary>
    /// <param name="unitName">单位名称</param>
    public void SwitchUnit(string unitName);
    /// <summary>
    /// 发动能力
    /// </summary>
    /// <param name="power">能力名称</param>
    public void Power(string power);
    /// <summary>
    /// 结束当前单位的回合
    /// </summary>
    public void EndTurn();
    /// <summary>
    /// 每个大回合Ego恢复
    /// </summary>
    public void RecoverEgo();
    /// <summary>
    /// 每个单位的回合开始时调用
    /// </summary>
    public void OnTurnStart();
    /// <summary>
    /// 每个单位的回合结束时调用
    /// </summary>
    public void OnTurnEnd();
}
