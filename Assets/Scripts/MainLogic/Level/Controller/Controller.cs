using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour, IController
{
    public string CotrollerKind { get; set; }
    public Dictionary<string, RuntimeUnitData> Units { get; set; }
    public string CurrentUnit { get; set; }
    public Powerable Powerable { get; set; }
    public EgoMachine EgoMachine { get; set; }

    public void Init()
    {

    }
    public void SwitchUnit(string unitName)
    {
        CurrentUnit = unitName;
    }
    public void Power()
    {

    }

    public void OnTurnStart()
    {

    }

    public void OnTurnEnd()
    {

    }
}
