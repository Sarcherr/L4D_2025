using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntry : IGameEntry
{
    public void Init()
    {
        LoadData();     
    }
    public void LoadData()
    {
        Debug.Log("LoadData");
        GlobalData.Init();
    }
}
