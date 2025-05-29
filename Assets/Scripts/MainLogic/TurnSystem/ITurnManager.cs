using System.Collections.Generic;

public interface ITurnManager
{
    public List<Turn> CurrentTurnQueue { get; set; }
    public List<Turn> BaseTurnQueue { get; set; }
    public Turn CurrentTurn { get; set; }

    public void RefreshQueue();

    public void AddToQueue(Turn turn);

    public void RemoveFromQueue(string name);

    public void NextTurn();

    public void OnTurnStart();

    public void OnTurnEnd();
}
