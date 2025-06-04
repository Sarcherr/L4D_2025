using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleQueueget : MonoBehaviour,IEndDragHandler
{
    private ScrollRect _rect;
    private void Awake()
    {
        _rect = this.GetComponent<ScrollRect>();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _rect.horizontalNormalizedPosition = 0f;
    }

    
    
}
