using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType { Burst, OutOfControl, Normal }
public enum StackRule { Replace, Stack, Refresh }
public enum GameEventType { TurnStart, TurnEnd, BeforeAttack, AfterAttack, OnDamage }
public enum BuffChangeAction { Add, Update, Remove } 

public class BuffConfig
{
    public int ID;                          // Buff唯一标识
    public string Name;                     // Buff名称
    public BuffType Type;                   // Buff类型
    public float Duration;                  // 持续时间（秒）
    public int MaxStacks = 3;               // 最大堆叠层数
    public StackRule StackRule;             // 堆叠规则
    public int Priority;                    // 优先级
    public List<GameEventType> TriggerEvents;// 触发事件类型
}

public class BuffInstance
{
    public BuffConfig Config;               // Buff配置引用
    public float RemainingTime;             // 剩余时间
    public int CurrentStacks = 1;           // 当前层数
    public object Source;                   // Buff来源（可选）
}



//下面这个是我自己想的关于这个buff引起的事件的类型 还没有实现

public enum EventEffectType { DamageModifier, StatBonus }
public enum EffectType { StatModifier, DamageOverTime, HealingOverTime, StatusEffect, CustomLogic }
