using System.Collections.Generic;
using UnityEngine;

public static class LevelDatabase
{
    public static Dictionary<string, LevelData> LevelData = new Dictionary<string, LevelData>();

    public static void Init()
    {
        //读取关卡信息
        //由于只读不存且读取数据均为string，采取读取csv的方式来进行
        // 加载CSV文件（注意：不要包含文件扩展名）
        TextAsset csvData = Resources.Load<TextAsset>("Data/LevelData");
        
        if(csvData == null)
        {
            Debug.LogError("CSV文件未找到！");
            return;
        }

        // 解析CSV数据
        LevelData = ParseCSV(csvData.text);
        
    }
    
    private static Dictionary<string, LevelData> ParseCSV(string csvText)
    {
        Dictionary<string, LevelData> result = new Dictionary<string, LevelData>();
        
        // 按行分割
        string[] lines = csvText.Split('\n');
        
        
        // 从第一行开始遍历数据
        for(int i = 0; i < lines.Length; i++)
        {
            if(string.IsNullOrWhiteSpace(lines[i])) continue;
            
            string[] values = lines[i].Split(',');
            
            string levelName = values[0];
            
            LevelData levelData = new LevelData();
            levelData.levelName = levelName;
            List<string> levelMonsterNames = new List<string>();

            for (int j = 1; j < values.Length; j++)
            {
                levelMonsterNames.Add(values[j]);
            }
            levelData.Monsters = levelMonsterNames;
            
            result.Add(levelName, levelData);
            //Debug.Log("LevelDataAdd:" + levelName + "," + levelData.Monsters[0]);
        }
        
        return result;
    }

    public static LevelData GetLevelData(string levelName)
    {
        if (LevelData.TryGetValue(levelName, out LevelData data))
        {
            return data;
        }

        Debug.LogError($"未找到关卡数据: {levelName}");
        return null;
    }
}
