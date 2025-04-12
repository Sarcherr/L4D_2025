using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour单例类，使用时继承该类，然后使用Instance属性获取单例
/// </summary>
/// <typeparam name="T">子类型</typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    _instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    _instance.Init();
                }
            }
            return _instance;
        }
    }

    protected virtual void Init()
    {

    }
}