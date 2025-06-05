using System.Collections.Generic;
using UnityEngine;

public class Controller : IController
{
    public string CotrollerKind { get; set; }

    public Dictionary<string, RuntimeUnitData> RuntimeUnits { get; set; }

    public string CurrentUnit { get; set; }

    public Powerable Powerable { get; set; }

    public EgoMachine EgoMachine { get; set; }

    private int _actionPoint;
    /// <summary>
    /// 行动力(初始默认为1)
    /// <para>为零时高亮结束回合按钮(todo)</para>
    /// </summary>
    public int ActionPoint
    {
        get => _actionPoint;
        set
        {
            _actionPoint = value;
            if (_actionPoint <= 0)
            {
                // todo: 高亮结束回合按钮
            }
        }
    }

    public Controller(string controllerKind, List<RuntimeUnitData> runtimeUnitDatas)
    {
        CotrollerKind = controllerKind;
        RuntimeUnits = new Dictionary<string, RuntimeUnitData>();

        if (runtimeUnitDatas != null)
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
        Debug.Log($"Switch to unit {CurrentUnit}");
        // 重置行动力
        ActionPoint = 1;
        OnTurnStart();


    }
    /// <summary>
    /// 发动能力
    /// </summary>
    /// <param name="power">能力名称</param>
    public void Power(string power)
    {
        if(ActionPoint > 0)
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
        else
        {
            Debug.LogWarning($"Unit {CurrentUnit} has no action points left to use power {power}.");
            // todo: 可以的话弹出提示UI
        }
    }
    /// <summary>
    /// 结束当前单位的回合
    /// </summary>
    public void EndTurn()
    {
        Debug.Log($"End turn for unit {CurrentUnit} with {ActionPoint} action points left.");
        ActionPoint = 0;
        OnTurnEnd();
        TurnManager.Instance.NextTurn();
    }

    /// <summary>
    /// 每个大回合Ego恢复
    /// </summary>
    public void RecoverEgo()
    {
        EgoMachine.RecoverEgo();
    }
    /// <summary>
    /// 每个单位的回合开始时调用
    /// </summary>
    public void OnTurnStart()
    {
        
    }
    /// <summary>
    /// 每个单位的回合结束时调用
    /// </summary>
    public void OnTurnEnd()
    {
        
    }
}
