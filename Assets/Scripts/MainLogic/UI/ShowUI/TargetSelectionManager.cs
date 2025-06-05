using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelectionManager : MonoSingleton<TargetSelectionManager>
{
    public LayerMask unitLayerMask;
    public GameObject ArrowGameObject;
    // 这三个字段会在 StartSelection 时被设置
    private string requiredAffiliation;         // 例如 "Enemy"、"Player"、"Unit"。表示只能选哪一类
    private GameObject sourceUnit;              // 施法者（用它来画箭头起点）
    private string powerName;                   // 施法的技能名称

    private bool isSelecting = false;
    private void Awake()
    {
    }

    /// <summary>
    /// 开始选择目标单位
    /// </summary>
    /// <param name="request"></param>
    public void StartSelection(PowerRequest request)
    {
        sourceUnit = GameObject.Find(request.Origin);
        var data = GlobalData.PowerDataDic[request.Name];
        powerName = request.Name;
        requiredAffiliation = data.uiControlType;
        isSelecting = true;

        Debug.Log($"开始选择目标单位：{sourceUnit.name} 使用技能 {powerName}，要求阵营：{requiredAffiliation}");
    }
    private void Update()
    {
        if (!isSelecting) return;

        Debug.Log("TargetSelectionManager: 正在选择目标单位...");

        // 仅监听鼠标左键点击
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

                        var message = new UIPowerMessage
                        {
                            Name = powerName,
                            Origin = sourceUnit.name,
                            Target = new List<string> { selectedName },
                            EgoComsumption = null,
                            NeedTarget = true,
                            NeedTargetEgo = false,
                        };

                        // 发送消息给 PowerManager
                        PowerManager.Instance.GeneratePower(message);

                        isSelecting = false;
                        sourceUnit = null;
                        requiredAffiliation = null;
                    }
                    else
                    {
                        Debug.Log("此单位不在允许选择的阵营：" + requiredAffiliation);
                    }
                }
            }
        }
    }
    private void DrawArrow(Vector3 fromWorld, Vector3 toWorld)
    {
        if (ArrowGameObject == null)
        {
            Debug.LogError("请在 Inspector 里把 arrowPrefab 挂上一个带 LineRenderer 的预制件。");
            return;
        }

        GameObject arrow = Instantiate(ArrowGameObject);
        LineRenderer lr = arrow.GetComponent<LineRenderer>();
        if (lr == null)
        {
            Debug.LogError("arrowPrefab 上必须挂有 LineRenderer 组件！");
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