using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 事件中心, 用于事件的订阅、取消订阅、通知
/// <para>所有需要注册的方法要求符合EventHandler格式，参数为(object sender, TEventArgs eventArgs)</para>
/// </summary>
public class EventCenter : Singleton<EventCenter>, IEventCenter
{
    /// <summary>
    /// 事件处理器字典
    /// </summary>
    private readonly Dictionary<string, List<Delegate>> _eventHandlers = new();

    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="eventHandler"></param>
    public void Subscribe<TEventArgs>(string eventName, EventHandler<TEventArgs> eventHandler)
    {
        if (_eventHandlers.TryGetValue(eventName, out var handlers))
        {
            handlers.Add(eventHandler);
        }
        else
        {
            _eventHandlers[eventName] = new(){ eventHandler };
        }
    }

    /// <summary>
    /// 取消订阅事件
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="eventHandler"></param>
    public void Unsubscribe<TEventArgs>(string eventName, EventHandler<TEventArgs> eventHandler)
    {
        if (_eventHandlers.TryGetValue(eventName, out var handlers))
        {
            handlers.Remove(eventHandler);
        }
    }

    /// <summary>
    /// 清空事件
    /// </summary>
    public void Clear()
    {
        _eventHandlers.Clear();
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    public void Notify<TEventArgs>(string eventName, object sender, TEventArgs eventArgs)
    {
        if (_eventHandlers.TryGetValue(eventName, out var handlers))
        {
            foreach (EventHandler<TEventArgs> handler in handlers.Cast<EventHandler<TEventArgs>>())
            {
                handler(sender, eventArgs);
            }
        }
    }
}
