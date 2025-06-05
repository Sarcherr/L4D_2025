using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelectionManager : MonoBehaviour
{
    public static TargetSelectionManager Instance;
    public LayerMask unitLayerMask;
    public GameObject ArrowGameObject;
    // �������ֶλ��� StartSelection ʱ������
    private Action<string> onUnitSelected;     // �ص���ѡ�к�����ִ���ȥ
    private string requiredAffiliation;         // ���� "Enemy"��"Player"��"Unit"����ʾֻ��ѡ��һ��
    private GameObject sourceUnit;              // ʩ���ߣ�����������ͷ��㣩

    private bool isSelecting = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartSelection(string affiliation, GameObject caster, Action<string> callback)
    {
        if (isSelecting)
        {
            Debug.LogWarning("����һ��Ŀ��ѡ����δ��ɣ��޷��ظ����� StartSelection��");
            return;
        }

        requiredAffiliation = affiliation;
        sourceUnit = caster;
        onUnitSelected = callback;
        isSelecting = true;


    }
    private void Update()
    {
        if (!isSelecting) return;

        // ���������������
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePoint2D = new Vector2(mouseWorld.x, mouseWorld.y);

            Collider2D hitCollider = Physics2D.OverlapPoint(mousePoint2D, unitLayerMask);
            if (hitCollider != null)
            {
                GameObject clickedObj = hitCollider.gameObject;
                AcUI uiComp = clickedObj.GetComponent<AcUI>();
                if (uiComp != null)
                {
                    string clickedAffiliation = uiComp.UnitAffiliation;
                    if (clickedAffiliation == requiredAffiliation)
                    {
                        string selectedName = clickedObj.name;
                        DrawArrow(sourceUnit.transform.position, clickedObj.transform.position);
                        onUnitSelected?.Invoke(selectedName);

                        isSelecting = false;
                        onUnitSelected = null;
                        sourceUnit = null;
                        requiredAffiliation = null;
                    }
                    else
                    {
                        Debug.Log("�˵�λ��������ѡ�����Ӫ��" + requiredAffiliation);
                    }
                }
            }
        }
    }
    private void DrawArrow(Vector3 fromWorld, Vector3 toWorld)
    {
        if (ArrowGameObject == null)
        {
            Debug.LogError("���� Inspector ��� arrowPrefab ����һ���� LineRenderer ��Ԥ�Ƽ���");
            return;
        }

        GameObject arrow = Instantiate(ArrowGameObject);
        LineRenderer lr = arrow.GetComponent<LineRenderer>();
        if (lr == null)
        {
            Debug.LogError("arrowPrefab �ϱ������ LineRenderer �����");
            Destroy(arrow);
            return;
        }

        Vector3 startPos = new Vector3(fromWorld.x, fromWorld.y, 0f);
        Vector3 endPos = new Vector3(toWorld.x, toWorld.y, 0f);

        lr.positionCount = 2;
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);

        Destroy(arrow, 0.5f);
    }
}