using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回合管理器
/// </summary>
public class TurnManager : Singleton<TurnManager>, ITurnManager
{
    /// <summary>
    /// 回合序列(使用单位名称排序，总体回合结束以字符串"End"为标志)
    /// <para>该序列为当前实际回合序列，作为CurrentQueue每次刷新的依据</para>
    /// </summary>
    public List<Turn> TurnQueue { get; set; }
    /// <summary>
    /// 当前回合序列，用于实时操作
    /// <para>发生变更时只会更改当前回合之后的部分，在该总体回合结束后刷新为TurnQueue的序列</para>
    /// </summary>
    public List<Turn> CurrentTurnQueue { get; set; }
    /// <summary>
    /// 当前回合
    /// </summary>
    public Turn CurrentTurn { get; set; }
    /// <summary>
    /// 当前总回合数(从1开始)
    /// </summary>
    public int CurrentGeneralTurn { get; set; }

    protected override void Init()
    {
        TurnQueue = new List<Turn>();
        CurrentTurnQueue = new List<Turn>();     
        RefreshQueue();
        CurrentTurn = CurrentTurnQueue[0];
        CurrentGeneralTurn = 1;  
    }

    /// <summary>
    /// 刷新回合序列
    /// </summary>
    public void RefreshQueue()
    {
        // 通过回合序列中单位名称查找所有单位EgoContainer
        // 以EgoContainer中Ego数量为依据对回合序列进行排序
        foreach (var pair in ControllerManager.Instance.AllEgoContainers)
        {
            if (ControllerManager.Instance.AllUnitData[pair.Value.BelongName].CurrentHealth > 0)
            {
                if (TurnQueue.Count == 0)
                {
                    TurnQueue.Add(new Turn() { Name = pair.Value.BelongName, IsExtraTurn = false });
                }
                else
                {
                    for (int i = 0; i < TurnQueue.Count; i++)
                    {
                        if (pair.Value.UnitEgo.Count > ControllerManager.Instance.AllEgoContainers[TurnQueue[i].Name].UnitEgo.Count)
                        {
                            TurnQueue.Insert(i, new Turn() { Name = pair.Value.BelongName, IsExtraTurn = false });
                            break;
                        }
                        else if (i == TurnQueue.Count - 1)
                        {
                            TurnQueue.Add(new Turn() { Name = pair.Value.BelongName, IsExtraTurn = false });
                            break;
                        }
                    }
                }
            }
            TurnQueue.Add(new Turn() { Name = "End", IsExtraTurn = false });

            for (int i = 0; i < TurnQueue.Count; i++)
            {
                var turn = TurnQueue[i];
                turn.TurnIndex = i;
                TurnQueue[i] = turn;
                CurrentTurnQueue.Add(TurnQueue[i]);
            }
        }
    }
    /// <summary>
    /// 添加回合
    /// </summary>
    /// <param name="turn"></param>
    public void AddToQueue(Turn turn)
    {
        // 添加额外回合时直接插入到当前回合之后，且只对CurrentTurnQueue进行操作
        // 否则按该回合名称对应的单位Ego数量插入到合适位置(同时更新CurrentTurnQueue和TurnQueue)
        if (turn.IsExtraTurn)
        {
            // 额外回合索引与当前回合一致
            turn.TurnIndex = CurrentTurn.TurnIndex;
            CurrentTurnQueue.Insert(CurrentTurn.TurnIndex + 1, turn);
        }
        else
        {
            // todo: 插入非额外回合(当前没有对应机制暂时不实现)
            // 非额外回合索引在当前回合之后
            // 注意插入后在当前回合之后的回合索引需要更新
        }
    }

    public void RemoveFromQueue(Turn turn)
    {

    }

    public void NextTurn()
    {

    }

    public void OnTurnStart()
    {

    }

    public void OnTurnEnd()
    {

    }
}

/// <summary>
/// 回合容器
/// </summary>
public struct Turn
{
    /// <summary>
    /// 单位名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 是否为额外回合
    /// </summary>
    public bool IsExtraTurn;
    /// <summary>
    /// 回合序号
    /// </summary>
    public int TurnIndex;
}
