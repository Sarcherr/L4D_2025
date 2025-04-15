using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
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
    }

    public void ShowUI()
    {

    }
    public void HideUI()
    {

    }

    #region UI_Level

    public void RefreshSkillButton()
    {

    }

    /// <summary>
    /// 向PowerManager发送UI消息
    /// </summary>
    public void SendMessageToPowerManager(UIPowerMessage message)
    {
        PowerManager.Instance.GeneratePower(message);
    }

    #endregion
}

/// <summary>
/// UI消息容器(提交给PowerManager)
/// </summary>
public struct UIPowerMessage
{

}
