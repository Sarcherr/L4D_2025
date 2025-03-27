using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public interface IController
{
    public Dictionary<string, UnitData> Units { get; set; }
    public UnitData CurrentUnit { get; set; }
    public void Init();
    public void SwitchUnit(string unitName);
    public void Power();
}
