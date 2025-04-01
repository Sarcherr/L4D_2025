using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController
{
    public Dictionary<string, RuntimeUnitData> Units { get; set; }
    public RuntimeUnitData CurrentUnit { get; set; }
    public Powerable Powerable { get; set; }
    public EgoMachine EgoMachine { get; set; }

    public void Init()
    {

    }
    public void SwitchUnit(string unitName)
    {
        CurrentUnit = Units[unitName];
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
