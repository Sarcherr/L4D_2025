using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class globalbutton : MonoBehaviour
{
    void Update()
    {
        // �������Ҽ����
        if (Input.GetMouseButtonDown(1))
        {
            // ȡ����ǰѡ�еĵ�λ
            if (AcUI.currentlySelectedUnit != null)
            {
                AcUI.currentlySelectedUnit.DeselectUnit();
            }
        }

        // ������������
        if (Input.GetMouseButtonDown(0))
        {
            // ����Ƿ�����UIԪ��
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // �������˿հ�����ȡ����ǰѡ�еĵ�λ
                if (AcUI.currentlySelectedUnit != null)
                {
                        AcUI.currentlySelectedUnit.DeselectUnit();
                }
            }
        }
    }
}
