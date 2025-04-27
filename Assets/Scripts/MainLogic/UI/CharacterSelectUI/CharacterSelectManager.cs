using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : Singleton<CharacterSelectManager>
{
    /// <summary>
    /// ��ѡ�����ҵ�λ�б�
    /// </summary>
    public List<string> SelectedCharacters = new List<string>();

    /// <summary>
    /// �����ǰѡ��Ľ�ɫ�����ں������
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
