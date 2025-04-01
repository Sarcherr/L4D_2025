using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IController
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
}