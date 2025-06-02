using System.Collections.Generic;

public class EgoContainer
{
    public EgoMachine EgoMachine { get; private set; }
    /// <summary>
    /// 容器所属单位名称
    /// </summary>
    public string BelongName { get; private set; }
    /// <summary>
    /// Ego条上限
    /// </summary>
    public int EgoLimit { get; private set; }
    /// <summary>
    /// Ego白条值(情感爆发阈值)
    /// </summary>
    public int EgoThreshold { get; private set; }
    /// <summary>
    /// 单位Ego列表(Ego数量同时为单位先攻值)
    /// </summary>
    public List<Ego> UnitEgo { get; private set; }

    public EgoContainer(RuntimeUnitData unitData, EgoMachine egoMachine)
    {
        EgoMachine = egoMachine;
        BelongName = unitData.Name;
        EgoLimit = unitData.EgoLimit;
        EgoThreshold = unitData.EgoThreshold;
        UnitEgo = new List<Ego>();
    }

    /// <summary>
    /// 获取Ego
    /// <para>同时判断是否达到情感爆发/失控的条件</para>
    /// </summary>
    /// <param name="egoList">待添加Ego列表</param>
    public void GainEgo(List<Ego> egoList)
    {
        foreach (var ego in egoList)
        {
            if (UnitEgo.Count == EgoThreshold + 1)
            {
                OnEgoBurst();
            }
            else if (UnitEgo.Count > EgoLimit)
            {
                OnEgoOutOfControl();
                break;
            }
            UnitEgo.Add(ego);
        }
    }

    /// <summary>
    /// 移除所有Ego
    /// </summary>
    /// <returns>本次移除的Ego列表</returns>
    public List<Ego> RemoveAllEgo()
    {
        List<Ego> removedEgos = new(UnitEgo);
        UnitEgo.Clear();

        return removedEgos;
    }
    /// <summary>  
    /// 移除Ego  
    /// <para>自由选择；在选中方法确定ID后才进行操作</para>  
    /// </summary>  
    /// <param name="removeIDs">待移除Ego编号</param>  
    /// <returns>本次移除的Ego列表</returns>  
    public List<Ego> RemoveEgo(List<int> removeIDs)
    {
        List<Ego> removedEgos = new();
        // 对待移除的ID进行排序，确保从大到小移除，避免索引偏移问题  
        removeIDs.Sort((a, b) => b.CompareTo(a));

        foreach (var num in removeIDs)

        {
            if (UnitEgo.Count == EgoThreshold)
            {
                OnEgoBelowThreshold();
            }
            removedEgos.Add(UnitEgo[num]);
            UnitEgo.RemoveAt(num);
        }

        return removedEgos;
    }
    /// <summary>
    /// 移除Ego
    /// <para>指定数量从Ego条尾部/头部顺序移除</para>
    /// </summary>
    /// <param name="removeCount">待移除Ego数量</param>
    /// <param name="beginFromEnd">是否从尾部开始</param>
    /// <param name="removeEgos">被移除的Ego列表</param>
    /// <returns>是否成功移除(待移除数量是否超过当前数量)</returns>
    public bool RemoveEgo(int removeCount, bool beginFromEnd, out List<Ego> removeEgos)
    {
        removeEgos = new();

        if (UnitEgo.Count >= removeCount)
        {
            if (beginFromEnd)
            {
                for (int i = 0; i < removeCount; i++)
                {
                    removeEgos.Add(UnitEgo[UnitEgo.Count - 1]);
                    UnitEgo.RemoveAt(UnitEgo.Count - 1);
                }
            }
            else
            {
                for (int i = 0; i < removeCount; i++)
                {
                    removeEgos.Add(UnitEgo[i]);
                    UnitEgo.RemoveAt(0);
                }
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// 转变所有Ego
    /// </summary>
    /// <param name="targetEgo"></param>
    /// <returns>被转化的Ego列表</returns>
    public List<Ego> TransformAllEgo(Ego targetEgo)
    {
        List<Ego> transformEgos = new(UnitEgo);

        for (int i = 0; i < UnitEgo.Count; i++)
        {
            UnitEgo[i] = targetEgo;
        }
        return transformEgos;
    }
    /// <summary>
    /// 转变Ego
    /// <para>自由选择；在选中方法确定ID后才进行操作</para>
    /// </summary>
    /// <param name="transformIDs">待转变Ego编号</param>
    /// <param name="targetEgo">目标类型</param>
    /// <returns>被转化的Ego列表</returns>
    public List<Ego> TransformEgo(List<int> transformIDs, Ego targetEgo)
    {
        List<Ego> transformEgos = new();

        foreach (int i in transformIDs)
        {
            transformEgos.Add(UnitEgo[i]);
            UnitEgo[i] = targetEgo;
        }

        return transformEgos;
    }
    /// <summary>
    /// 转变Ego 
    /// <para>指定数量从Ego条尾部/头部顺序转变</para>
    /// </summary>
    /// <param name="transformCount">待转变Ego数量</param>
    /// <param name="beginFromEnd">是否从尾部开始</param>
    /// <param name="targetEgo">目标类型</param>
    /// <param name="transformEgos">被转变的Ego列表</param>
    /// <returns>是否成功转变(待转变数量是否超过当前数量)</returns>
    public bool TransformEgo(int transformCount, bool beginFromEnd, Ego targetEgo, out List<Ego> transformEgos)
    {
        transformEgos = new();

        if (UnitEgo.Count >= transformCount)
        {
            if (beginFromEnd)
            {
                for (int i = 0; i < transformCount; i++)
                {
                    transformEgos.Add(UnitEgo[UnitEgo.Count - 1 - i]);
                    UnitEgo[UnitEgo.Count - 1 - i] = targetEgo;
                }
            }
            else
            {
                for (int i = 0; i < transformCount; i++)
                {
                    transformEgos.Add(UnitEgo[i]);
                    UnitEgo[i] = targetEgo;
                }
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// 附加Ego
    /// <para>只对普通Ego(Normal)生效</para>
    /// </summary>
    /// <param name="attachCount">待附加Ego数量</param>
    /// <param name="beginFromEnd">是否从尾部开始</param>
    /// <param name="targetEgo">目标类型</param>
    /// <param name="attachEgos">被附加的Ego列表</param>
    /// <returns>是否成功附加(待附加数量是否超过当前数量)</returns>
    public bool AttachEgo(int attachCount, bool beginFromEnd, Ego targetEgo, out List<Ego> attachEgos)
    {
        attachEgos = new();

        //附加只对普通Ego(Normal)生效
        if (UnitEgo.Count >= attachCount)
        {
            if (beginFromEnd)
            {
                for (int i = 0; i < attachCount; i++)
                {
                    if (UnitEgo[UnitEgo.Count - 1 - i].EgoType == "Normal")
                    {
                        attachEgos.Add(UnitEgo[UnitEgo.Count - 1 - i]);
                        UnitEgo[UnitEgo.Count - 1 - i] = targetEgo;
                    }
                }
            }
            else
            {
                for (int i = 0; i < attachCount; i++)
                {
                    if (UnitEgo[i].EgoType == "Normal")
                    {
                        attachEgos.Add(UnitEgo[i]);
                        UnitEgo[i] = targetEgo;
                    }
                }
            }
            return true;
        }

        return false;
    }

    /// <summary>
    /// 转移Ego
    /// </summary>
    /// <param name="exchangeIDs">待转移Ego编号</param>
    /// <param name="target">转移目标</param>
    /// <returns>转移Ego列表</returns>
    public List<Ego> ExchangeEgo(List<int> exchangeIDs, string target)
    {
        List<Ego> exchangeEgos = RemoveEgo(exchangeIDs);
        ControllerManager.Instance.AllEgoContainers[target].GainEgo(exchangeEgos);
        return exchangeEgos;
    }

    /// <summary>
    /// 清除Ego
    /// </summary>
    /// <param name="purifyIDs">待清除Ego编号</param>
    /// <returns>清除Ego列表</returns>
    public List<Ego> PurifyEgo(List<int> purifyIDs)
    {
        Ego targetEgo = new()
        {
            EgoType = "Normal",
            HostName = BelongName,
            CanConsume = true,
        };

        List<Ego> purifyEgos = new();
        foreach (int i in purifyIDs)
        {
            purifyEgos.Add(UnitEgo[i]);
            UnitEgo[i] = targetEgo;
        }

        return purifyEgos;
    }

    /// <summary>
    /// 消耗Ego
    /// <para>自由选择；在选中方法确定ID后才进行操作</para>  
    /// </summary>
    /// <param name="consumeIDs">待消耗Ego编号</param>
    /// <returns>消耗Ego列表</returns>
    public List<Ego> ConsumeEgo(List<int> consumeIDs)
    {
        List<Ego> consumeEgos = new();
        foreach (int i in consumeIDs)
        {
            if (UnitEgo[i].CanConsume)
            {
                consumeEgos.Add(UnitEgo[i]);
            }
        }

        consumeEgos = RemoveEgo(consumeIDs);
        EgoMachine.TriggerEgo(consumeEgos, "Consume", BelongName);
        return consumeEgos;
    }
    /// <summary>
    /// 消耗Ego
    /// <para>指定数量从Ego条尾部/头部顺序消耗</para>
    /// </summary>
    /// <param name="consumeCount">待消耗Ego数量</param>
    /// <param name="beginFromEnd">是否从尾部开始</param>
    /// <param name="consumeEgos">被消耗Ego列表</param>
    /// <returns>是否成功消耗(待消耗数量是否超过当前数量)</returns>
    public bool ConsumeEgo(int consumeCount, bool beginFromEnd, out List<Ego> consumeEgos)
    {
        consumeEgos = new();
        List<int> ego2RemoveIDs = new();
        int cannotConsumeCount = UnitEgo.FindAll(x => x.CanConsume == false).Count;
        // 注意判断时除去不能消耗的Ego数量
        if (UnitEgo.Count - cannotConsumeCount >= consumeCount)
        {
            if (beginFromEnd)
            {
                for (int i = 0; i < consumeCount; i++)
                {
                    if (UnitEgo[UnitEgo.Count - 1 - i].CanConsume)
                    {
                        ego2RemoveIDs.Add(UnitEgo.Count - 1 - i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < consumeCount; i++)
                {
                    if (UnitEgo[i].CanConsume)
                    {
                        ego2RemoveIDs.Add(i);
                    }
                }
            }

            consumeEgos = RemoveEgo(ego2RemoveIDs);
            EgoMachine.TriggerEgo(consumeEgos, "Consume", BelongName);
            return true;
        }

        return false;
    }
    /// <summary>  
    /// 消耗Ego  
    /// <para>指定数量优先消耗指定类型的Ego，默认从末尾开始</para>  
    /// </summary>  
    /// <param name="consumeCount">待消耗Ego数量</param>  
    /// <param name="priorEgoType">优先消耗的Ego类型</param>  
    /// <param name="consumeEgos">被消耗的Ego列表</param>  
    /// <returns>是否成功消耗(待消耗数量是否超过当前数量)</returns>  
    public bool ConsumeEgo(int consumeCount, string priorEgoType, out List<Ego> consumeEgos)
    {
        consumeEgos = new();
        List<int> ego2RemoveIDs = new();
        int cannotConsumeCount = UnitEgo.FindAll(x => x.CanConsume == false).Count;

        // 注意判断时除去不能消耗的Ego数量  
        if (UnitEgo.Count - cannotConsumeCount >= consumeCount)
        {
            int consumed = 0;

            // 优先消耗指定类型的Ego  
            for (int i = UnitEgo.Count - 1; i >= 0 && consumed < consumeCount; i--)
            {
                if (UnitEgo[i].CanConsume && UnitEgo[i].EgoType == priorEgoType)
                {
                    ego2RemoveIDs.Add(i);
                    consumed++;
                }
            }

            // 如果优先类型不足，继续消耗其他类型的Ego  
            for (int i = UnitEgo.Count - 1; i >= 0 && consumed < consumeCount; i--)
            {
                if (UnitEgo[i].CanConsume && UnitEgo[i].EgoType != priorEgoType)
                {
                    ego2RemoveIDs.Add(i);
                    consumed++;
                }
            }
  
            consumeEgos = RemoveEgo(ego2RemoveIDs);
            EgoMachine.TriggerEgo(consumeEgos, "Consume", BelongName);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 初始化时获得Ego(仅针对敌人)
    /// </summary>
    public void OnEgoInit()
    {
        List<Ego> recoverEgos = new();
        for (int i = 0; i < GlobalData.RuntimeUnitDataDic[BelongName].EgoStartValue; i++)
        {
            if (UnitEgo.Count < EgoLimit)
            {
                recoverEgos.Add(new Ego()
                {
                    EgoType = "Normal",
                    HostName = BelongName,
                    CanConsume = true,
                });
            }
        }

        GainEgo(recoverEgos);
    }

    /// <summary>
    /// 每个大回合开始时Ego恢复
    /// </summary>
    public void OnGeneralEgoRecover()
    {
        List<Ego> recoverEgos = new();
        for (int i = 0; i < GlobalData.RuntimeUnitDataDic[BelongName].EgoRecoverValue; i++)
        {
            if (UnitEgo.Count < EgoLimit)
            {
                recoverEgos.Add(new Ego()
                {
                    EgoType = "Normal",
                    HostName = BelongName,
                    CanConsume = true,
                });
            }
        }

        GainEgo(recoverEgos);
    }

    /// <summary>
    /// Ego超出阈值(情感爆发)
    /// </summary>
    public void OnEgoBurst()
    {
        ControllerManager.Instance.AllRuntimeUnitData[BelongName].IsBurst = true;

        EgoMachine.TriggerEgo(UnitEgo, "Burst", BelongName);
    }
    /// <summary>
    /// Ego溢出上限(失控)
    /// </summary>
    public void OnEgoOutOfControl()
    {
        ControllerManager.Instance.AllRuntimeUnitData[BelongName].IsOutOfControl = true;

        EgoMachine.TriggerEgo(UnitEgo, "OutOfControl", BelongName);
    }
    /// <summary>
    /// Ego低于阈值
    /// </summary>
    public void OnEgoBelowThreshold()
    {
        ControllerManager.Instance.AllRuntimeUnitData[BelongName].IsBurst = false;
        // todo: BuffMachine移除相关buff
    }
    /// <summary>
    /// Ego失控结束
    /// </summary>
    public void OnEgoOutOfControlEnd()
    {
        ControllerManager.Instance.AllRuntimeUnitData[BelongName].IsOutOfControl = false;
        // todo: BuffMachine移除相关buff
    }
}
