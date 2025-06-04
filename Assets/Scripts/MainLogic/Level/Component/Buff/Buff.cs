/// <summary>
/// Buff基类
/// </summary>
public class Buff
{
    /// <summary>
    /// buff名称
    /// </summary>
    public string Name;
    /// <summary>
    /// buff回合阶段
    /// </summary>
    public TurnStage TurnStage;
    /// <summary>
    /// buff类型
    /// </summary>
    public BuffType Type;

    /// <summary>
    /// buff所属单位名称
    /// </summary>
    public string BelongName;
    /// <summary>
    /// buff施加者名称
    /// </summary>
    public string OriginName;

    /// <summary>
    /// buff剩余回合数
    /// <para>-1为无限持续</para>
    /// </summary>
    public int TurnCount;
    /// <summary>
    /// buff层数
    /// </summary>
    public int BuffCount;

    public Buff(string name, TurnStage turnStage, BuffType type, 
        string belongName, string originName, int turnCount, int buffCount)
    {
        Name = name;
        TurnStage = turnStage;
        Type = type;
        BelongName = belongName;
        OriginName = originName;
        TurnCount = turnCount;
        BuffCount = buffCount;
    }
}

/// <summary>
/// 回合阶段
/// </summary>
public enum TurnStage
{
    /// <summary>
    /// 回合开始阶段
    /// </summary>
    Start,
    /// <summary>
    /// 行动前阶段
    /// </summary>
    Action_before,
    /// <summary>
    /// 行动后阶段
    /// </summary>
    Action_after,
    /// <summary>
    /// 攻击前阶段
    /// </summary>
    Attack_before,
    /// <summary>
    /// 攻击后阶段
    /// </summary>
    Attack_after,
    /// <summary>
    /// 回合结束阶段
    /// </summary>
    End,
    /// <summary>
    /// 立即生效
    /// </summary>
    Current
}

public enum BuffType
{
    /// <summary>
    /// 普通buff
    /// </summary>
    Normal,
    /// <summary>
    /// 情感爆发buff
    /// </summary>
    Burst,
    /// <summary>
    /// 失控buff
    /// </summary>
    OutOfControl,
}
