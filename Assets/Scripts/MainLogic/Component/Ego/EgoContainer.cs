using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoContainer
{
    public List<Ego> UnitEgo;

    public void Init()
    {
        EventCenter.Instance.Subscribe<EgoArgs>("gainEgo", GainEgo);
    }

    public void GainEgo<EgoArgs>(object sender, EgoArgs egoArgs)
    {

    }
}

public class EgoArgs : EventArgs
{
    public int TargetID;
}
