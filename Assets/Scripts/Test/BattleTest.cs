using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<string> names = new List<string> { "Dolora", "Nora", "Jia_Baoyu" };
        BattleManager.Instance.InitBattle(names, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
