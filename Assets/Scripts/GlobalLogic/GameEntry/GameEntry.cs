using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEntry
{
    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        LoadData();     
    }
    public static void LoadData()
    {
        Debug.Log("LoadData");
        GlobalData.Init();
    }
}
