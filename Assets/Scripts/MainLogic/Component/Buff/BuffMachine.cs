using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffManager
{
    private static BuffManager _instance;
    public static BuffManager Instance => _instance ??= new BuffManager();

    private readonly Dictionary<int, List<BuffInstance>> _characterBuffs = new();

    public event Action<int, BuffInstance, BuffChangeAction> OnBuffChanged;

    private BuffManager()
    {
        InitializeEventHandlers();
        SubscribeToGameEvents();
    }

    private void InitializeEventHandlers()
    {

    }

    private void SubscribeToGameEvents()
    {
        // 这里订阅游戏系统中的事件，例如 EventSystem.Subscribe(GameEventType.TurnStart, OnGameEvent);
    }

    // 添加或更新 Buff
    public void AddBuff(ICharacter character, BuffConfig config, object source = null)
    {
        if (!_characterBuffs.TryGetValue(character.InstanceID, out var buffList))
        {
            buffList = new List<BuffInstance>();
            _characterBuffs[character.InstanceID] = buffList;
        }

        var existingBuff = buffList.FirstOrDefault(b => b.Config.ID == config.ID);
        bool isNewBuff = existingBuff == null;

        if (existingBuff != null)
        {
            // 根据堆叠规则更新数据
            switch (config.StackRule)
            {
                case StackRule.Replace:
                    existingBuff.RemainingTime = config.Duration;
                    break;
                case StackRule.Stack when existingBuff.CurrentStacks < config.MaxStacks:
                    existingBuff.CurrentStacks++;
                    existingBuff.RemainingTime = config.Duration;
                    break;
                case StackRule.Refresh:
                    existingBuff.RemainingTime = config.Duration;
                    break;
            }

            // 通知控制层，buff数据发生了更新
            OnBuffChanged?.Invoke(character.InstanceID, existingBuff, BuffChangeAction.Update);
        }
        else
        {
            var newBuff = new BuffInstance
            {
                Config = config,
                RemainingTime = config.Duration,
                Source = source
            };
            buffList.Add(newBuff);

            // 通知控制层，新增了 buff
            OnBuffChanged?.Invoke(character.InstanceID, newBuff, BuffChangeAction.Add);
        }
    }

    public void Update()
    {
        // 遍历所有角色的 buff，处理过期逻辑
        foreach (var pair in _characterBuffs)
        {
            foreach (var buff in pair.Value.ToList())
            {
                buff.RemainingTime -= Time.deltaTime;
                if (buff.RemainingTime <= 0)
                {
                    RemoveBuff(pair.Key, buff);
                }
            }
        }
    }

    private void RemoveBuff(int characterId, BuffInstance buff)
    {
        if (_characterBuffs.TryGetValue(characterId, out var buffList))
        {
            buffList.Remove(buff);
            // 通知控制层，buff 被移除
            OnBuffChanged?.Invoke(characterId, buff, BuffChangeAction.Remove);
        }
    }

    // 这段代码主要是用来响应游戏中的某个全局事件，然后根据该事件去检查角色身上的所有 Buff，看是否有设定要对这个事件作出响应，从而触发相应的更新通知。
    private void OnGameEvent(GameEventType eventType, ICharacter character, object eventData)
    {
        if (!_characterBuffs.TryGetValue(character.InstanceID, out var buffList))
            return;

        foreach (var buff in buffList.ToList())
        {
            if (buff.Config.TriggerEvents.Contains(eventType))
            {
                OnBuffChanged?.Invoke(character.InstanceID, buff, BuffChangeAction.Update);
            }
        }
    }

    // 清理角色 Buff 时触发通知
    public void ClearCharacterBuffs(ICharacter character)
    {
        if (_characterBuffs.TryGetValue(character.InstanceID, out var buffList))
        {
            foreach (var buff in buffList)
            { 
                OnBuffChanged?.Invoke(character.InstanceID, buff, BuffChangeAction.Remove);
            }
            _characterBuffs.Remove(character.InstanceID);
        }
    }
}