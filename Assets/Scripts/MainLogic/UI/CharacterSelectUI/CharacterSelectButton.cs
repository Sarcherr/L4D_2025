using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    /// <summary>
    /// 该按钮代表的角色是否正被选择
    /// </summary>
    public bool IsSelected { get; private set; }
    /// <summary>
    /// 角色ID
    /// </summary>
    public int CharacterID { get; private set; }
    /// <summary>
    /// 角色名称
    /// </summary>
    public string CharacterName { get; private set; }
    /// <summary>
    /// 角色标识
    /// </summary>
    public Image CharacterIcon { get; private set; }
    public Button CharacterButton { get; private set; }
    public TextMeshProUGUI CharacterNameText { get; private set; }
    public Sprite SelectedCharacterIcon;
    public Sprite UnselectedCharacterIcon;



    private void Awake()
    {
        IsSelected = false;
        CharacterButton = gameObject.GetComponent<Button>();
        CharacterNameText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        CharacterIcon = gameObject.GetComponent<Image>();
        CharacterButton.onClick.AddListener(SelectCharacter);

        // 能力按钮ID直接截取按钮gameobject名称格式Power_ID中末尾的ID数字
        string[] name = gameObject.name.Split('_');
        if (name.Length > 1)
        {
            CharacterID = int.Parse(name[1]);
        }
        else
        {
            Debug.LogError("CharacterSelectButton name format error, please check the name format.");
        }


        UIManager.Instance.RegisterUI("CharacterSelectUI.CharacterSelectButton", CharacterButton);
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
            CharacterIcon.sprite = SelectedCharacterIcon;
        }
        else
        {
            CharacterIcon.sprite = UnselectedCharacterIcon;
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
