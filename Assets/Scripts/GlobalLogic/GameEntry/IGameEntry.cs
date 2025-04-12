using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEntry
{
    public void Init();
    public void LoadUnitData();
    public void LoadGameData();
}
