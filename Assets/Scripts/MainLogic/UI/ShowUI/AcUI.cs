using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AcUI : MonoBehaviour, IPointerClickHandler
{
    public GameObject actionArrow; // �ж���ͷ
    public GameObject selectionRing; // ѡ��Բ��
    private string unitName; // ��λ����
    private RuntimeUnitData unitData; // ��λ����ʱ����

    public static AcUI currentlySelectedUnit;

    private void Awake()
    {
        if (actionArrow != null) actionArrow.SetActive(false);
        if (selectionRing != null) selectionRing.SetActive(false);
    }
    void Start()
    {
        // �Ӹ������ȡ��λ����
        unitName = gameObject.name;
        // �� ControllerManager ��ȡ��λ����
        if (ControllerManager.Instance.AllRuntimeUnitData.TryGetValue(unitName, out RuntimeUnitData data))
        {
            unitData = data;
        }
        else
        {
            Debug.LogError($"UnitUIManager: Cannot find unit data for {unitName}");
        }
    }
    void Update()
    {
        // ����Ƿ��ǵ�ǰ�ж���λ
        if (TurnManager.Instance.CurrentTurn.Name == unitName)
        {
            if (actionArrow != null) actionArrow.SetActive(true);
        }
        else
        {
            if (actionArrow != null) actionArrow.SetActive(false);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // ��������ѡ�иõ�λ
            SelectUnit();
        }
    }
    public void SelectUnit()
    {
        if (currentlySelectedUnit != null && currentlySelectedUnit != this)
        {
            currentlySelectedUnit.DeselectUnit();
        }

        // ���õ�ǰ��λΪѡ��״̬
        currentlySelectedUnit = this;
        if (selectionRing != null) selectionRing.SetActive(true);
    }
    public void DeselectUnit()
    {
        if (selectionRing != null) selectionRing.SetActive(false);
        if (currentlySelectedUnit == this)
        {
            currentlySelectedUnit = null;
        }
    }
}
