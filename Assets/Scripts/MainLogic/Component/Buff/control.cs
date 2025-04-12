using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffControl : MonoBehaviour
{
    private void Start()
    {
        BuffManager.Instance.OnBuffChanged += HandleBuffChange;
    }

    private void OnDestroy()
    {
        BuffManager.Instance.OnBuffChanged -= HandleBuffChange;
    }

    // 根据 Buff 的变化类型执行具体行为
    private void HandleBuffChange(int characterId, BuffInstance buff, BuffChangeAction action)
    {
        ICharacter character = GetCharacterById(characterId);
        if (character == null) return;

        switch (action)
        {
            case BuffChangeAction.Add:
                Debug.Log($"角色 {characterId} 新增Buff: {buff.Config.Name}");
                // 示例：更新属性、播放特效、UI提示等
                ApplyBuffBehavior(character, buff);
                break;
            case BuffChangeAction.Update:
                Debug.Log($"角色 {characterId} 更新Buff: {buff.Config.Name}, 当前层数: {buff.CurrentStacks}");
                RefreshBuffBehavior(character, buff);
                break;
            case BuffChangeAction.Remove:

                Debug.Log($"角色 {characterId} 移除Buff: {buff.Config.Name}");
                RemoveBuffBehavior(character, buff);
                break;
        }
    }

    private ICharacter GetCharacterById(int characterId)
    {
        // 这里返回当前场景中对应角色对象
        return null;
    }

    private void ApplyBuffBehavior(ICharacter character, BuffInstance buff)
    {
        // 根据 buff 的类型或其它字段，执行具体行为
        switch (buff.Config.Type)
        {
            case BuffType.Burst:
                break;
            case BuffType.OutOfControl:
                break;
            case BuffType.Normal:
                break;
        }

    }

    private void RefreshBuffBehavior(ICharacter character, BuffInstance buff)
    {

    }

    private void RemoveBuffBehavior(ICharacter character, BuffInstance buff)
    {
        // 当buff移除时清除效果、还原属性、停止特效等
        switch (buff.Config.Type)
        {
            case BuffType.Burst:
                break;
            case BuffType.OutOfControl:
                break;
            case BuffType.Normal:
                break;
        }
    }
}