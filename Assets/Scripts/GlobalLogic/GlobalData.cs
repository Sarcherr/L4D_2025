using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

public static class GlobalData
{
    private const string _unitDataPath = "Data/UnitData";
    private const string _powerDataPath = "Data/PowerData";

    /// <summary>
    /// 储存所有单位信息，索引为单位名称
    /// </summary>
    public static readonly Dictionary<string, UnitData> UnitDataDic = new();
    /// <summary>
    /// 储存所有玩家单位的运行时数据，索引为单位名称
    /// </summary>
    public static readonly Dictionary<string, RuntimeUnitData> RuntimeUnitDataDic = new();
    /// <summary>
    /// 储存所有能力信息，索引为技能名称(仅技能次数限制)
    /// </summary>
    public static readonly Dictionary<string, PowerData> PowerDataDic = new();

    public static void Init()
    {
        var unitData = LoadData(_unitDataPath);
        var powerData = LoadData(_powerDataPath);

        // 注意PowerDataDic的反序列化要在UnitDataDic之前
        DeSerializeBytes_PowerData(powerData);
        DeSerializeBytes_UnitData(unitData);

        // 测试用，记得删除

        GenerateRuntimeUnitData();

        // 测试用，记得删除
    }

    /// <summary>
    /// 生成运行时单位数据
    /// <para>用于开始新游戏</para>
    /// </summary>
    public static void GenerateRuntimeUnitData()
    {
        Debug.Log("/// GenerateRuntimeUnitData ///");

        List<string> playerUnits = UnitDataDic.Where(x => x.Value.UnitKind == "Player").Select(x => x.Key).ToList();

        if (playerUnits.Count == 0)
        {
            Debug.LogError("GenerateRuntimeUnitData Error: No player units found");
            return;
        }

        foreach (var unit in playerUnits)
        {
            RuntimeUnitData runtimeUnitData = new RuntimeUnitData();
            runtimeUnitData.CopyData(UnitDataDic[unit]);

            Debug.Log($"Generating runtime data for unit: {unit}");
            RuntimeUnitDataDic.Add(unit, runtimeUnitData);
        }

        Debug.Log("/// GenerateRuntimeUnitData End ///");
    }

    /// <summary>
    /// 加载运行时单位数据
    /// <para>从存档文件中加载</para>
    /// </summary>
    public static void LoadRuntimeUnitData()
    {

    }

    /// <summary>
    /// 加载数据源文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static TextAsset LoadData(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        if (textAsset == null)
        {
            Debug.LogError($"LoadData Error: {path} not found");
            return null;
        }

        return textAsset;
    }

    /// <summary>
    /// 反序列化单位数据二进制文件，导入UnitDataDic
    /// <para>注意该过程要晚于于DeSerializeBytes_PowerData</para>
    /// </summary>
    /// <param name="data"></param>
    public static void DeSerializeBytes_UnitData(TextAsset data)
    {
        if (data == null)
        {
            Debug.LogError("DeSerializeBytes_UnitData Error: data is null");
            return;
        }

        if (data.bytes == null || data.bytes.Length == 0)
        {
            Debug.LogError("DeSerializeBytes_UnitData Error: data is empty");
            return;
        }

        Debug.Log("/// DeSerializeBytes_UnitData ///");

        UnitDataDic.Clear();

        MemoryStream ms = new MemoryStream(data.bytes);
        BinaryReader br = new BinaryReader(ms);
        int count = br.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            Debug.Log($"UnitData: {i + 1}/{count}");

            string unitKind = br.ReadString();
            string name = br.ReadString();
            string name_CN = br.ReadString();
            int attack = int.Parse(br.ReadString());
            float dogeChance = float.Parse(br.ReadString());
            int health = int.Parse(br.ReadString());
            float critChance = float.Parse(br.ReadString());
            float critRate = float.Parse(br.ReadString());
            float resistanceRate = float.Parse(br.ReadString());
            float hitChance = float.Parse(br.ReadString());
            float damageReductionRate = float.Parse(br.ReadString());
            int egoLimit = int.Parse(br.ReadString());
            int egoThreshold = int.Parse(br.ReadString());
            int egoStartValue = int.Parse(br.ReadString());
            int egoRecoverValue = int.Parse(br.ReadString());

            UnitData unitData = new UnitData()
            {
                UnitKind = unitKind,
                Name = name,
                Name_CN = name_CN,
                PowerList = PowerDataDic.Values.Where(x => x.belongName == name).ToList(),
                Attack = attack,
                DogeChance = dogeChance,
                Health = health,
                CritChance = critChance,
                CritRate = critRate,
                ResistanceRate = resistanceRate,
                HitChance = hitChance,
                DamageReductionRate = damageReductionRate,
                EgoLimit = egoLimit,
                EgoThreshold = egoThreshold,
                EgoStartValue = egoStartValue,
                EgoRecoverValue = egoRecoverValue,
            };

            if (!UnitDataDic.TryAdd(unitData.Name, unitData))
            {
                Debug.LogError($"DeSerializeBytes_UnitData Error: {unitData.Name} already exists");
            }
            else
            {
                Debug.Log($"UnitData: {unitData.Name} added");
            }
        }
        Debug.Log("/// DeSerializeBytes_UnitData End ///");
    }

    /// <summary>
    /// 反序列化能力数据二进制文件，导入PowerDataDataDic
    /// <para>注意该过程要先于DeSerializeBytes_UnitData</para>
    /// </summary>
    /// <param name="data"></param>
    public static void DeSerializeBytes_PowerData(TextAsset data)
    {
        if (data == null)
        {
            Debug.LogError("DeSerializeBytes_PowerData Error: data is null");
            return;
        }

        if (data.bytes == null || data.bytes.Length == 0)
        {
            Debug.LogError("DeSerializeBytes_PowerData Error: data is empty");
            return;
        }

        Debug.Log("/// DeSerializeBytes_PowerData ///");

        PowerDataDic.Clear();

        MemoryStream ms = new MemoryStream(data.bytes);
        BinaryReader br = new BinaryReader(ms);
        int count = br.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            Debug.Log($"PowerData: {i + 1}/{count}");

            string belongName = br.ReadString();
            string name = br.ReadString();
            string name_CN = br.ReadString();
            int limit = int.Parse(br.ReadString());
            int egoConsumption = int.Parse(br.ReadString());
            string uiControlKind = br.ReadString();
            string description = br.ReadString();

            PowerData powerData = new PowerData()
            {
                name = name,
                name_CN = name_CN,
                belongName = belongName,
                limit = limit,
                egoConsumption = egoConsumption,
                uiControlKind = uiControlKind,
                description = description
            };
            if (!PowerDataDic.TryAdd(powerData.name, powerData))
            {
                Debug.LogError($"DeSerializeBytes_PowerData Error: {powerData.name} already exists");
            }
            else
            {
                Debug.Log($"PowerData: {powerData.name} added");
                Debug.Log($"Name_CN: {powerData.name_CN}, BelongName: {powerData.belongName}, Limit: {powerData.limit}, EgoConsumption: {powerData.egoConsumption}, UIControlKind: {powerData.uiControlKind}");
            }
        }
    }
}
