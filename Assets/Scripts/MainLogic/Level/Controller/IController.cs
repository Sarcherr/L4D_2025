using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public interface IController
{
    public string CotrollerKind { get; set; }
    public Dictionary<string, RuntimeUnitData> Units { get; set; }
    public string CurrentUnit { get; set; }
    public Powerable Powerable { get; set; }
    public EgoMachine EgoMachine { get; set; }
    public void SwitchUnit(string unitName);
    public void Power(string power);
}
