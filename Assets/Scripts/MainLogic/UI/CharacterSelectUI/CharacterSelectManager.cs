using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : Singleton<CharacterSelectManager>
{
    /// <summary>
    /// 被选择的玩家单位列表
    /// </summary>
    public List<string> SelectedCharacters = new List<string>();

    /// <summary>
    /// 输出当前选择的角色，便于后续检查
    /// </summary>
    public void CheckSelectedCharacters()
    {
        string allSelectedCharacters = "";

        foreach (string character in SelectedCharacters)
        {
            allSelectedCharacters += character + ", ";
        }
        
        Debug.Log("current selected characters:" + allSelectedCharacters);
    }
    
    
}
