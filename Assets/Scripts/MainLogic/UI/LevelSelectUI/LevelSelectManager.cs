using System.Collections.Generic;

public class LevelSelectManager: Singleton<LevelSelectManager>
{
    public List<string> MonsterNames = new List<string>();

    public void SetMonsterNames(string levelName)
    {
        MonsterNames.Clear();
        LevelData data = LevelDatabase.GetLevelData(levelName);
        MonsterNames = data.Monsters;
    }
}