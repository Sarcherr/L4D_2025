using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public interface IController
{
    public Dictionary<string, RuntimeUnitData> Units { get; set; }
    public RuntimeUnitData CurrentUnit { get; set; }
    public Powerable Powerable { get; set; }
    public EgoMachine EgoMachine { get; set; }
    public void Init();
    public void SwitchUnit(string unitName);
    public void Power();
}
