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
    /// 单位Ego列表
    /// </summary>
    public List<Ego> UnitEgo {  get; private set; }

    public EgoContainer(UnitData unitData)
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
                OnEgoOutOfLimit();
                break;
            }
            UnitEgo.Add(ego);
        }
    }
    /// <summary>
    /// 移除Ego
    /// <para>自由选择；在选中方法确定ID后才进行操作，故不做可行性判断</para>
    /// </summary>
    /// <param name="removeCount">待移除Ego编号</param>
    /// <returns>本次移除的Ego列表</returns>
    public List<Ego> RemoveEgo(List<int> removeCount)
    {
        List<Ego> removedEgos = new();
        foreach(var num in removeCount)
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
    /// <para>指定数量从Ego条尾部顺序移除</para>
    /// </summary>
    /// <param name="removeNum">待移除Ego数量</param>
    /// <param name="removeEgos">被移除的Ego列表</param>
    /// <returns></returns>
    public bool RemoveEgo(int removeNum, out List<Ego> removeEgos)
    {
        removeEgos = new();

        if(UnitEgo.Count >= removeNum)
        {
            for(int i = 0; i < removeNum; i++)
            {
                removeEgos.Add(UnitEgo[UnitEgo.Count - 1]);
                UnitEgo.RemoveAt(UnitEgo.Count - 1);
            }
            return true;
        }

        return false;
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
    public void OnEgoOutOfLimit()
    {

    }
    /// <summary>
    /// Ego低于阈值
    /// </summary>
    public void OnEgoBelowThreshold()
    {

    }
}
