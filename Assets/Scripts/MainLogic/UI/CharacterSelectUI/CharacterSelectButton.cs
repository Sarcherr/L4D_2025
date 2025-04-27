using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    /// <summary>
    /// �ð�ť����Ľ�ɫ�Ƿ�����ѡ��
    /// </summary>
    public bool IsSelected {get; private set;}
    /// <summary>
    /// ��ɫID
    /// </summary>
    public int CharacterID {get; private set;}
    /// <summary>
    /// ��ɫ����
    /// </summary>
    public string CharacterName {get; private set;}
    /// <summary>
    /// ��ɫ��ʶ
    /// </summary>
    public Image CharacterIcon {get; private set;}
    public Button CharacterButton {get; private set;}
    public TextMeshProUGUI CharacterNameText {get; private set;}
    
    
    
    private void Awake()
    {
        IsSelected = false;
        CharacterButton = gameObject.GetComponent<Button>();
        CharacterNameText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        CharacterIcon = gameObject.GetComponent<Image>();
        CharacterButton.onClick.AddListener(SelectCharacter);
        
        // ������ťIDֱ�ӽ�ȡ��ťgameobject���Ƹ�ʽPower_ID��ĩβ��ID����
        string[] name = gameObject.name.Split('_');
        if (name.Length > 1)
        {
            CharacterID = int.Parse(name[1]);
        }
        else
        {
            Debug.LogError("CharacterSelectButton name format error, please check the name format.");
        }
        
        
        UIManager.Instance.RegisterUI("CharacterSelectUI.CharacterSelectButton",CharacterButton);
    }

    public void Refresh(string characterName/*, Sprite characterIcon*/)
    {
        CharacterName = characterName;
        CharacterNameText.text = characterName;
        // CharacterIcon.sprite = characterIcon;

        UpdateButtonColor();
    }

    private void UpdateButtonColor()
    {
        if (IsSelected)
        {
            CharacterIcon.color = Color.gray;
        }
        else
        {
            CharacterIcon.color = Color.white;
        }
    }

    private void SelectCharacter()
    {
        if (!IsSelected)
        {
            
            if (CharacterSelectManager.Instance.SelectedCharacters.Count < 4)
            {
                IsSelected = true;
                CharacterSelectManager.Instance.SelectedCharacters.Add(CharacterName);
                UpdateButtonColor();
                CharacterSelectManager.Instance.CheckSelectedCharacters();
            }
            else
            {
                Debug.Log("SelectedCharacters is more than 4 characters.");
                UpdateButtonColor();
                CharacterSelectManager.Instance.CheckSelectedCharacters();
            }
            
        }
        else
        {
            IsSelected = false;
            CharacterSelectManager.Instance.SelectedCharacters.Remove(CharacterName);
            UpdateButtonColor();
            CharacterSelectManager.Instance.CheckSelectedCharacters();
        }
    }
}
