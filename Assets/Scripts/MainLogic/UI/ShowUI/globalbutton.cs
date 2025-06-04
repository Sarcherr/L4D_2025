using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class globalbutton : MonoBehaviour
{
    void Update()
    {
        // 检测鼠标右键点击
        if (Input.GetMouseButtonDown(1))
        {
            // 取消当前选中的单位
            if (AcUI.currentlySelectedUnit != null)
            {
                AcUI.currentlySelectedUnit.DeselectUnit();
            }
        }

        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 检查是否点击了UI元素
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // 如果点击了空白区域，取消当前选中的单位
                if (AcUI.currentlySelectedUnit != null)
                {
                        AcUI.currentlySelectedUnit.DeselectUnit();
                }
            }
        }
    }
}
