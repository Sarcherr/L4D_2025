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
    public void SwitchUnit(string unitName);
    public void Power(string power);
    public void EndTurn();
}
