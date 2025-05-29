using System.Collections;
using System.Collections.Generic;
using MainLogic.UI.LevelSelectUI;
using UnityEngine;
using UI.LevelUI.Controller;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    // 所有字典索引按命名空间索引分组，省略UI开头
    // 如：LevelUI.Controller.PowerButton -> LevelUI.Controller.PowerButton
    /// <summary>
    /// 按钮字典
    /// </summary>
    public Dictionary<string, List<Button>> ButtonsDic { get; private set; }
    /// <summary>
    /// 滑动条字典
    /// </summary>
    public Dictionary<string, List<Slider>> SlidersDic { get; private set; }
    /// <summary>
    /// 切换字典
    /// </summary>
    public Dictionary<string, List<Toggle>> TogglesDic { get; private set; }

    protected override void Init()
    {
        ButtonsDic = new Dictionary<string, List<Button>>();
        SlidersDic = new Dictionary<string, List<Slider>>();
        TogglesDic = new Dictionary<string, List<Toggle>>();
    }

    /// <summary>
    /// 注册UI
    /// </summary>
    /// <typeparam name="T">UI类型</typeparam>
    /// <param name="UIgroup">对应UI组别</param>
    /// <param name="targetUI">待注册UI</param>
    public void RegisterUI<T>(string UIgroup, T targetUI)
    {
        if (targetUI is Button)
        {
            if (!ButtonsDic.ContainsKey(UIgroup))
            {
                ButtonsDic.Add(UIgroup, new List<Button>());
            }
            ButtonsDic[UIgroup].Add(targetUI as Button);
        }
        else if (targetUI is Slider)
        {
            if (!SlidersDic.ContainsKey(UIgroup))
            {
                SlidersDic.Add(UIgroup, new List<Slider>());
            }
            SlidersDic[UIgroup].Add(targetUI as Slider);
        }
        else if (targetUI is Toggle)
        {
            if (!TogglesDic.ContainsKey(UIgroup))
            {
                TogglesDic.Add(UIgroup, new List<Toggle>());
            }
            TogglesDic[UIgroup].Add(targetUI as Toggle);
        }

        Debug.Log($"RegisterUI: {UIgroup} - {(targetUI as MonoBehaviour).gameObject.name}");
    }
    /// <summary>
    /// 注销UI
    /// </summary>
    /// <typeparam name="T">UI类型</typeparam>
    /// <param name="UIgroup">UI对应组别</param>
    /// <param name="targetUI">待注销UI</param>
    public void UnregisterUI<T>(string UIgroup, T targetUI)
    {
        if (targetUI is Button)
        {
            if (ButtonsDic.ContainsKey(UIgroup))
            {
                ButtonsDic[UIgroup].Remove(targetUI as Button);
            }
        }
        else if (targetUI is Slider)
        {
            if (SlidersDic.ContainsKey(UIgroup))
            {
                SlidersDic[UIgroup].Remove(targetUI as Slider);
            }
        }
        else if (targetUI is Toggle)
        {
            if (TogglesDic.ContainsKey(UIgroup))
            {
                TogglesDic[UIgroup].Remove(targetUI as Toggle);
            }
        }

        Debug.Log($"UnregisterUI: {UIgroup} - {(targetUI as MonoBehaviour).gameObject.name}");
    }

    public void ShowUI()
    {

    }
    public void HideUI()
    {

    }

    #region UI_Level

    /// <summary>
    /// 刷新技能按钮
    /// </summary>
    public void RefreshSkillButton()
    {
        foreach (var button in ButtonsDic["LevelUI.Controller.PowerButton"])
        {
            // 只有前四个技能按钮需要刷新
            if (button.GetComponent<PowerButton>().PowerID <= 4)
            {
                string powerName = ControllerManager.Instance.
                    AllRuntimeUnitData[ControllerManager.Instance.Player.CurrentUnit].
                    PowerRecord[button.GetComponent<PowerButton>().PowerID - 1].powerData.name;
                string powerDescription = ControllerManager.Instance.
                    AllRuntimeUnitData[ControllerManager.Instance.Player.CurrentUnit].
                    PowerRecord[button.GetComponent<PowerButton>().PowerID - 1].powerData.description;

                Debug.Log($"PowerName: {powerName} PowerDescription: {powerDescription}");
                button.GetComponent<PowerButton>().Refresh(powerName, powerDescription);
            }
        }
    }

    // todo: 带单位名称参数的刷新技能按钮方法
    public void RefreshSkillButton(string targetUnit)
    {
        foreach (var button in ButtonsDic["LevelUI.Controller.PowerButton"])
        {
            // 只有前四个技能按钮需要刷新
            if (button.GetComponent<PowerButton>().PowerID <= 4)
            {
                var targetUnitData = ControllerManager.Instance.AllRuntimeUnitData[targetUnit];
                var powerIndex = button.GetComponent<PowerButton>().PowerID - 1;

                string powerName;
                string powerDescription;
                
                if (powerIndex > targetUnitData.PowerRecord.Count)
                {
                    powerName = " ";
                    powerDescription = " ";
                }
                else
                {
                    powerName = targetUnitData.PowerRecord[powerIndex].powerData.name;
                    powerDescription = targetUnitData.PowerRecord[powerIndex].powerData.description;
                    
                }
                Debug.Log($"PowerName: {powerName} PowerDescription: {powerDescription}");
                button.GetComponent<PowerButton>().Refresh(powerName, powerDescription);
            }
        }
    }

    /// <summary>
    /// 向PowerManager发送UI消息
    /// </summary>
    public void SendMessageToPowerManager(UIPowerMessage message)
    {
        PowerManager.Instance.GeneratePower(message);
    }

    #endregion 
    
    #region UI_CharacterSelect

    public void RefreshCharacterSelectButton()
    {
        // 清空已选择的角色列表
        CharacterSelectManager.Instance.SelectedCharacters.Clear();

        Dictionary<int,Button> characterSelectButtons = new Dictionary<int,Button>();
        
        foreach (var button in ButtonsDic["CharacterSelectUI.CharacterSelectButton"])
        {
            characterSelectButtons.Add(button.GetComponent<CharacterSelectButton>().CharacterID,button);
        }
        
        int buttonIndex = 1;
        foreach (var pair in GlobalData.RuntimeUnitDataDic)
        {
             characterSelectButtons[buttonIndex].GetComponent<CharacterSelectButton>().Refresh(pair.Key);
             buttonIndex++;
        }
    }
    
    #endregion
    
    #region UI_LevelSelect

    public void RefreshLevelSelectButton()
    {
        LevelSelectManager.Instance.MonsterNames.Clear();
        
        Dictionary<int,Button> levelSelectButtons = new Dictionary<int,Button>();

        foreach (var button in ButtonsDic["LevelSelectUI.LevelSelectButton"])
        {
            levelSelectButtons.Add(button.GetComponent<LevelSelectButton>().LevelID,button);
        }
        
        int buttonIndex = 1;
        foreach (var pair in LevelDatabase.LevelData)
        {
            levelSelectButtons[buttonIndex].GetComponent<LevelSelectButton>().Refresh(pair.Key);
            buttonIndex++;
        }
        
        
    }
    
    #endregion
} 

/// <summary>
/// UI消息容器(提交给PowerManager)
/// </summary>
public struct UIPowerMessage
{

}
