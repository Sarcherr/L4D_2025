using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventCenter
{
    public void Subscribe<TEventArgs>(string eventName, EventHandler<TEventArgs> eventHandler);

    public void Unsubscribe<TEventArgs>(string eventName, EventHandler<TEventArgs> eventHandler);

    public void Clear();

    public void Notify<TEventArgs>(string eventName, object sender, TEventArgs eventArgs);
}
