using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : IController
{
    /// <summary>
    /// 控制器类型(Player/Enemy)
    /// </summary>
    public string CotrollerKind { get; set; }
    /// <summary>
    /// 出战单位
    /// </summary>
    public Dictionary<string, RuntimeUnitData> Units { get; set; }
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

    public Controller(string controllerKind, List<RuntimeUnitData> runtimeUnitDatas)
    {
        CotrollerKind = controllerKind;
        Units = new Dictionary<string, RuntimeUnitData>();
        foreach (var unitData in runtimeUnitDatas)
        {
            Units.Add(unitData.Name, unitData);
        }
        Powerable = new Powerable();
        EgoMachine = new EgoMachine(this);
    }

    /// <summary>
    /// 切换当前行动单位
    /// </summary>
    /// <param name="unitName">单位名称</param>
    public void SwitchUnit(string unitName)
    {
        CurrentUnit = unitName;
    }

    /// <summary>
    /// 发动能力
    /// </summary>
    /// <param name="power">能力名称</param>
    public void Power(string power)
    {
        Debug.Log($"Unit {CurrentUnit} use power {power}");

        PowerRequest request = new PowerRequest()
        {
            Name = power,
            Origin = CurrentUnit,
            UnitKind = Units[CurrentUnit].UnitKind
        };

        PowerManager.Instance.HandleRequest(request);
    }

    /// <summary>
    /// 每个大回合Ego恢复
    /// </summary>
    public void RecoverEgo()
    {
        EgoMachine.RecoverEgo();
    }

    public void OnTurnStart()
    {
        
    }

    public void OnTurnEnd()
    {

    }
}
