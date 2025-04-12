using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    int InstanceID { get; }
    //void ApplyTemporaryModifier(StatType stat, float value);
    //void RemoveTemporaryModifier(StatType stat, float value);
    //void TriggerCustomEffect(string methodName, object parameters);
    //float GetBaseStat(StatType stat);
}