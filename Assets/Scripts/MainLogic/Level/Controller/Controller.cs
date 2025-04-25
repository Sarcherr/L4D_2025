using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : IController
{
    public string CotrollerKind { get; set; }

    public Dictionary<string, RuntimeUnitData> RuntimeUnits { get; set; }

    public string CurrentUnit { get; set; }

    public Powerable Powerable { get; set; }

    public EgoMachine EgoMachine { get; set; }

    public Controller(string controllerKind, List<RuntimeUnitData> runtimeUnitDatas)
    {
        CotrollerKind = controllerKind;
        RuntimeUnits = new Dictionary<string, RuntimeUnitData>();

        if(runtimeUnitDatas != null)
        {
            foreach (var unitData in runtimeUnitDatas)
            {
                RuntimeUnits.Add(unitData.Name, unitData);
            }
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
            UnitKind = RuntimeUnits[CurrentUnit].UnitKind
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
