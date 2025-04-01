using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoContainer
{
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

    public EgoContainer(RuntimeUnitData unitData)
    {
        BelongName = unitData.Name;
        EgoLimit = unitData.EgoLimit;
        EgoThreshold = unitData.EgoThreshold;
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
            if(egoList.Count == EgoThreshold)
            {
                OnEgoBurst();
            }
            else if(egoList.Count == EgoLimit)
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
    /// <param name="removeID">待移除Ego编号</param>
    /// <returns>本次移除的Ego列表</returns>
    public List<Ego> RemoveEgo(List<int> removeID)
    {
        List<Ego> removedEgos = new();
        foreach(var num in removeID)
        {
            if(UnitEgo.Count == EgoThreshold)
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

        if(UnitEgo.Count >= removeCount)
        {
            if(beginFromEnd)
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
                    UnitEgo.RemoveAt(i);
                }
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// 转变所有Ego
    /// </summary>
    /// <param name="targetType"></param>
    public void TransformAllEgo(Ego targetEgo)
    {
        for(int i = 0; i < UnitEgo.Count; i++)
        {
            UnitEgo[i] = targetEgo;
        }
    }
    /// <summary>
    /// 转变Ego
    /// <para>自由选择；在选中方法确定ID后才进行操作</para>
    /// </summary>
    /// <param name="transformIDs">待转变Ego编号</param>
    /// <param name="targetEgo">目标类型</param>
    public void TransformEgo(List<int> transformIDs, Ego targetEgo)
    {
        foreach (int i in transformIDs)
        {
            UnitEgo[i] = targetEgo;
        }
    }
    

    /// <summary>
    /// Ego超出阈值(情感爆发)
    /// </summary>
    public void OnEgoBurst()
    {

    }
    /// <summary>
    /// Ego溢出上限(失控)
    /// </summary>
    public void OnEgoOutOfControl()
    {

    }
    /// <summary>
    /// Ego低于阈值
    /// </summary>
    public void OnEgoBelowThreshold()
    {

    }
}
