using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButton : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SkipTurn);
    }

    public void SkipTurn()
    {
        TurnManager.Instance.NextTurn();
    }
}
