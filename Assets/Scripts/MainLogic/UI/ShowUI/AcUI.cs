using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class AcUI : MonoBehaviour, IPointerClickHandler
{
    public GameObject actionArrow; // 行动箭头
    public GameObject selectionRing; // 选中圆环
    private string unitName; // 单位名称
    private RuntimeUnitData unitData; // 单位运行时数据

    public bool isInit = false; // 是否已初始化

    public static AcUI currentlySelectedUnit;
    public string UnitAffiliation;
    // 添加事件系统
    public static event Action<string> OnUnitSelected;

    public void Init()
    {
        // 从父对象获取单位名称
        unitName = gameObject.name;
        // 从 ControllerManager 获取单位数据
        if (ControllerManager.Instance.AllRuntimeUnitData.TryGetValue(unitName, out RuntimeUnitData data))
        {
            unitData = data;
            isInit = true;
            return;
        }
        else
        {
            Debug.LogError($"UnitUIManager: Cannot find unit data for {unitName}");
            isInit = false;
            return;
        }
    }

    private void Awake()
    {
        if (actionArrow != null) actionArrow.SetActive(false);
        if (selectionRing != null) selectionRing.SetActive(false);
        UnitAffiliation = ControllerManager.Instance.AllRuntimeUnitData[unitName].UnitKind;
    }
    void Start()
    {

    }
    void Update()
    {
        if (!isInit)
        {
            Init();
        }
        else
        {
            // 检查是否是当前行动单位
            if (TurnManager.Instance.CurrentTurn.Name == unitName)
            {
                if (actionArrow != null) actionArrow.SetActive(true);
            }
            else
            {
                if (actionArrow != null) actionArrow.SetActive(false);
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            // 左键点击，选中该单位

            SelectUnit();
        }
    }
    public void SelectUnit()
    {
        if (currentlySelectedUnit != null && currentlySelectedUnit != this)
        {
            currentlySelectedUnit.DeselectUnit();
        }


        // 设置当前单位为选中状态
        currentlySelectedUnit = this;
        if (selectionRing != null) selectionRing.SetActive(true);
        
        // 触发单位选择事件
        OnUnitSelected?.Invoke(unitName);
    }
    
    public void DeselectUnit()
    {
        if (selectionRing != null) selectionRing.SetActive(false);
    }
}
