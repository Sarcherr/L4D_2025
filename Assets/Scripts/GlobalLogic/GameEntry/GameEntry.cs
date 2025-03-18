using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntry : IGameEntry
{
    public void Init()
    {
        LoadGameData();
        LoadUnitData();     
    }
    public void LoadUnitData()
    {
        Debug.Log("LoadUnitData");
    }
    public void LoadGameData()
    {
        Debug.Log("LoadGameData");
    }
}
