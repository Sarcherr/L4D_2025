using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class UIManager : Singleton<UIManager>
{
    protected override void Init()
    {

    }
    public void ShowUI()
    {

    }
    public void HideUI()
    {

    }
    /// <summary>
    /// 向PowerManager发送UI消息
    /// </summary>
    public void SendMessageToPowerManager(UIPowerMessage message)
    {
        PowerManager.Instance.GeneratePower(message);
    }
}

/// <summary>
/// UI消息容器(提交给PowerManager)
/// </summary>
public struct UIPowerMessage
{

}
