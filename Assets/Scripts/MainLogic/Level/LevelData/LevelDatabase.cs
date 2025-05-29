using System.Collections.Generic;
using UnityEngine;

public static class LevelDatabase
{
    public static Dictionary<string, LevelData> LevelData = new Dictionary<string, LevelData>();

    public static void Init()
    {
        //��ȡ�ؿ���Ϣ
        //����ֻ�������Ҷ�ȡ���ݾ�Ϊstring����ȡ��ȡcsv�ķ�ʽ������
        // ����CSV�ļ���ע�⣺��Ҫ�����ļ���չ����
        TextAsset csvData = Resources.Load<TextAsset>("Data/LevelData");
        
        if(csvData == null)
        {
            Debug.LogError("CSV�ļ�δ�ҵ���");
            return;
        }

        // ����CSV����
        LevelData = ParseCSV(csvData.text);
        
    }
    
    private static Dictionary<string, LevelData> ParseCSV(string csvText)
    {
        Dictionary<string, LevelData> result = new Dictionary<string, LevelData>();
        
        // ���зָ�
        string[] lines = csvText.Split('\n');
        
        
        // �ӵ�һ�п�ʼ��������
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

        Debug.LogError($"δ�ҵ��ؿ�����: {levelName}");
        return null;
    }
}
