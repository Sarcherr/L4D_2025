using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回合管理器
/// </summary>
public class TurnManager : Singleton<TurnManager>, ITurnManager
{
    // 所有回合序列均按单位Ego数量从大到小降序排列
    /// <summary>
    /// 回合序列(使用单位名称排序，总体回合结束以字符串"End"为标志)
    /// <para>该序列为当前实际回合序列，作为CurrentQueue每次刷新的依据</para>
    /// </summary>
    public List<Turn> BaseTurnQueue { get; set; }
    /// <summary>
    /// 当前回合序列，用于实时操作(使用单位名称排序，总体回合结束以字符串"End"为标志)
    /// <para>发生变更时只会更改当前回合之后的部分，在该总体回合结束后刷新为BaseTurnQueue的序列</para>
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
        BaseTurnQueue = new List<Turn>();
        CurrentTurnQueue = new List<Turn>();
    }

    /// <summary>
    /// 刷新回合序列(用于初始化)
    /// </summary>
    public void RefreshQueue()
    {
        // 通过回合序列中单位名称查找所有单位EgoContainer
        // 以EgoContainer中Ego数量为依据对回合序列进行排序
        foreach (var pair in ControllerManager.Instance.AllEgoContainers)
        {
            // 插入回合对应单位数据
            var data = ControllerManager.Instance.AllRuntimeUnitData[pair.Value.BelongName];
            Turn turn2Add = new Turn()
            {
                Name = pair.Value.BelongName,
                UnitKind = data.UnitKind,
                IsExtraTurn = false,
                EgoValue = pair.Value.UnitEgo.Count
            };

            if (data.CurrentHealth > 0)
            {
                if (BaseTurnQueue.Count == 0)
                {
                    BaseTurnQueue.Add(turn2Add);
                }
                else
                {
                    for (int i = 0; i < BaseTurnQueue.Count; i++)
                    {
                        if (pair.Value.UnitEgo.Count > BaseTurnQueue[i].EgoValue)
                        {
                            BaseTurnQueue.Insert(i, turn2Add);
                            break;
                        }
                        else if (i == BaseTurnQueue.Count - 1)
                        {
                            BaseTurnQueue.Add(turn2Add);
                            break;
                        }
                    }
                }
            }
        }

        BaseTurnQueue.Add(new Turn() { Name = "End", IsExtraTurn = false });

        for (int i = 0; i < BaseTurnQueue.Count; i++)
        {
            var turn = BaseTurnQueue[i];
            BaseTurnQueue[i] = turn;
            CurrentTurnQueue.Add(BaseTurnQueue[i]);
        }

        CurrentTurn = CurrentTurnQueue[0];
        CurrentGeneralTurn = 1;

        ControllerManager.Instance.SwitchUnit(CurrentTurn.Name);
        Debug.Log($"CurrentTurn: {CurrentTurn.Name} - {CurrentTurn.EgoValue} - {CurrentTurn.UnitKind} - {CurrentGeneralTurn}");
        Debug.Log("CurrentTurnQueue: " + string.Join(", ", CurrentTurnQueue.ConvertAll(t => t.Name).ToArray()));

        // 刷新UI
        UIManager.Instance.RefreshSkillButton();
    }
    /// <summary>
    /// 添加回合(目前只用于添加额外回合)
    /// </summary>
    /// <param name="turn"></param>
    public void AddToQueue(Turn turn)
    {
        // 添加额外回合时直接插入到当前回合之后，且只对CurrentTurnQueue进行操作
        // 否则按该回合名称对应的单位Ego数量插入到合适位置(同时更新CurrentTurnQueue和BaseTurnQueue)
        if (turn.IsExtraTurn)
        {
            turn.UnitKind = CurrentTurn.UnitKind;
            // 插入到当前回合后
            CurrentTurnQueue.Insert(1, turn);
        }
        else
        {
            // todo: 插入非额外回合(当前没有对应机制暂时不实现)
            // 非额外回合索引在当前回合之后
            // 注意插入后在当前回合之后的回合索引需要更新
        }
    }
    /// <summary>
    /// 从回合序列中移除
    /// </summary>
    /// <param name="name">单位名称</param>
    public void RemoveFromQueue(string name)
    {
        CurrentTurnQueue.RemoveAll(t => t.Name == name);
        BaseTurnQueue.RemoveAll(t => t.Name == name);
        //for (int i = 0; i < CurrentTurnQueue.Count; i++)
        //{
        //    if (CurrentTurnQueue[i].Name == name)
        //    {
        //        CurrentTurnQueue.RemoveAt(i);
        //        break;
        //    }
        //}
        //for (int i = 0; i < TurnQueue.Count; i++)
        //{
        //    if (TurnQueue[i].Name == name)
        //    {
        //        TurnQueue.RemoveAt(i);
        //        break;
        //    }
        //}
    }
    /// <summary>
    /// 更新回合(用于Ego数量改变时更新回合序列)
    /// </summary>
    /// <param name="name">单位名称</param>
    public void UpdateTurn(string name)
    {
        List<(Turn turn, int index)> turnPairs = new();

        for (int i = 0; i < CurrentTurnQueue.Count; i++)
        {
            if (CurrentTurnQueue[i].Name == name)
            {
                var turn = CurrentTurnQueue[i];
                turn.EgoValue = ControllerManager.Instance.AllEgoContainers[name].UnitEgo.Count;
                CurrentTurnQueue[i] = turn;
                turnPairs.Add((turn, i));
            }
        }

        MoveTurn(turnPairs, "current");
        turnPairs.Clear();

        for (int i = 0; i < BaseTurnQueue.Count; i++)
        {
            if (BaseTurnQueue[i].Name == name)
            {
                var turn = BaseTurnQueue[i];
                turn.EgoValue = ControllerManager.Instance.AllEgoContainers[name].UnitEgo.Count;
                BaseTurnQueue[i] = turn;
                turnPairs.Add((turn, i));
            }
        }

        MoveTurn(turnPairs, "base");
    }
    /// <summary>
    /// 移动回合
    /// <para>同时向UI发送信息</para>
    /// </summary>
    /// <param name="turnPairs">回合信息</param>
    /// <param name="targetQueue">目标回合序列</param>
    private void MoveTurn(List<(Turn turn, int index)> turnPairs, string targetQueue)
    {
        if (targetQueue == "current")
        {
            foreach (var (turn, index) in turnPairs)
            {
                CurrentTurnQueue.RemoveAt(index);
                for (int i = 0; i < CurrentTurnQueue.Count; i++)
                {
                    if (turn.EgoValue > CurrentTurnQueue[i].EgoValue)
                    {
                        CurrentTurnQueue.Insert(i, turn);
                        // todo: 向UI发送信息
                        break;
                    }
                    else if (i == CurrentTurnQueue.Count - 1)
                    {
                        CurrentTurnQueue.Add(turn);
                        // todo: 向UI发送信息
                        break;
                    }
                }
            }
        }
        else if (targetQueue == "base")
        {
            foreach (var (turn, index) in turnPairs)
            {
                if (!turn.IsExtraTurn)
                {
                    BaseTurnQueue.RemoveAt(index);
                    for (int i = 0; i < BaseTurnQueue.Count; i++)
                    {
                        if (turn.EgoValue > BaseTurnQueue[i].EgoValue)
                        {
                            BaseTurnQueue.Insert(i, turn);
                            // todo: 向UI发送信息
                            break;
                        }
                        else if (i == BaseTurnQueue.Count - 1)
                        {
                            BaseTurnQueue.Add(turn);
                            // todo: 向UI发送信息
                            break;
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// 进入下一回合
    /// </summary>
    public void NextTurn()
    {
        // todo: 向UI发送信息,回合序列左移一格
        // 将旧的CurrentTurn从CurrentTurnQueue中移除
        // CurrentTurn取CurrentTurnQueue中的下一个回合
        CurrentTurnQueue.RemoveAt(0);
        CurrentTurn = CurrentTurnQueue[0];

        if (CurrentTurn.Name == "End")
        {
            CurrentGeneralTurn++;
            CurrentTurnQueue = new List<Turn>(BaseTurnQueue);
            CurrentTurn = CurrentTurnQueue[0];
        }
        else
        {
            ControllerManager.Instance.SwitchUnit(CurrentTurn.Name);
        }

        // 刷新UI
        UIManager.Instance.RefreshSkillButton();
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
    /// 单位阵营(Player/Enemy)
    /// </summary>
    public string UnitKind;
    /// <summary>
    /// 是否为额外回合
    /// </summary>
    public bool IsExtraTurn;
    /// <summary>
    /// 回合对应单位先攻值
    /// </summary>
    public int EgoValue;
}
