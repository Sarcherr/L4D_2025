using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnManager
{
    public List<Turn> CurrentTurnQueue { get; set; }
    public List<Turn> TurnQueue { get; set; }
    public Turn CurrentTurn { get; set; }
    public int CurrentGeneralTurn { get; set; }

    public void RefreshQueue();

    public void AddToQueue(Turn turn);

    public void RemoveFromQueue(Turn turn);

    public void NextTurn();

    public void OnTurnStart();

    public void OnTurnEnd();
}
