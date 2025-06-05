public struct PowerData
{
    /// <summary>
    /// 所属单位名称
    /// </summary>
    public string belongName;
    /// <summary>
    /// 能力名称
    /// </summary>
    public string name;
    /// <summary>
    /// 能力名称(中文)
    /// </summary>
    public string name_CN;
    /// <summary>
    /// 使用次数上限(0为无上限)
    /// </summary>
    public int limit;
    /// <summary>
    /// Ego消耗
    /// </summary>
    public int egoConsumption;
    /// <summary>
    /// UI操作类型
    /// </summary>
    public string uiControlType;
    /// <summary>
    /// 技能描述
    /// </summary>
    public string description;

    ///// <summary>
    ///// 随机系数最小值
    ///// </summary>
    //public float minRandomFactor;
    ///// <summary>
    ///// 随机系数最大值
    ///// </summary>
    //public float maxRandomFactor;
}