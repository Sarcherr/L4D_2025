using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController
{
    public Dictionary<string, UnitData> Units { get; set; }
    public UnitData CurrentUnit { get; set; }
    public Powerable Powerable { get; set; }
    public Attackable Attackable { get; set; }
    public Skillable Skillable { get; set; }
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
}
