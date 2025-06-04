using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class btest : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject contentobj;
    private RectTransform content;
    public RectTransform viewport;

    [Header("预制体设置")]
    public List<GameObject> prefabPool;
    public List<GameObject> enemyPool;
    public GameObject enditem;
    public GameObject ExtraItem;
    public int visibleCount = 6;
    public float fadeDuration = 0.5f;
    public float scrollSpeed = 0.1f;
    public float interval = 2f;

    private Dictionary<string, int> playfab=new Dictionary<string, int>();
    private Dictionary<string, int> enemyfab=new Dictionary<string, int>();
    private List<GameObject> currentItems = new List<GameObject>();
    public List<Tuple<string,int>> forList = new List<Tuple<string,int>>();
    private List<Tuple<string,int>> originalForList = new List<Tuple<string, int>>();
  
    // 标识是否触发了“时光回溯”特殊逻辑
    private bool isTimeReversalTriggered = false;

    void Awake()
    {
        if (contentobj != null)
        {
            content = contentobj.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("contentobj 没有赋值！");
        }
    }

    private void Start()
    {
        if (forList == null || forList.Count == 0)
        {
            GenerateForList();
        }

        for (int i = 0; i < visibleCount; i++)
        {
            GameObject item = Instantiate(GetPrefabUsingForList(), content);
            currentItems.Add(item);
        }

        StartCoroutine(TurnLoop());
    }

    /// <summary>
    /// 主循环，每隔一定时间处理一次移除最左边的物体，并从 forList 中取出一个新的物体加入最右侧
    /// </summary>
    /// <returns></returns>
    private IEnumerator TurnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            HandleLeftOutAndRightIn();
        }
    }

    /// <summary>
    /// 处理左侧物体移出和右侧新物体加入的逻辑
    /// </summary>
    private void HandleLeftOutAndRightIn()
    {
        if (TurnManager.Instance.CurrentTurn.Name == "End")
        {
            //处理每个回合结束之后的ego添加和其他回合结束效果
            return;
        }
        if (currentItems.Count == 0) return;
        if (isTimeReversalTriggered)
        {
            // 保留最左侧物体，不进行销毁处理
            GameObject nleftItem = currentItems[0];
            Vector3 pos = nleftItem.transform.localPosition;
            Transform parent = nleftItem.transform.parent;
            Destroy(nleftItem);
            currentItems.RemoveAt(0);
            GameObject extrai = Instantiate(ExtraItem, parent);
            extrai.transform.localPosition = pos;
            currentItems.Insert(0, extrai);

            // 清除其余显示的物体（销毁并从列表中移除）
            for (int i = currentItems.Count - 1; i > 0; i--)
            {
                Destroy(currentItems[i]);
                currentItems.RemoveAt(i);
            }

            for (int i = 0; i < visibleCount; i++)
            {
                
                if (i < originalForList.Count)
                {
                    int prefabIndex = originalForList[i].Item2;
                    GameObject unewItem = Instantiate(prefabPool[prefabIndex], content);
                    currentItems.Add(unewItem);
                }
                else
                {
                    GameObject unewItem = Instantiate(enditem, content);
                    currentItems.Add(unewItem);
                    break;
                }
            }
            forList.Clear();
            isTimeReversalTriggered = false;
            return;
        }
        // 移除最左侧的物体
        GameObject leftItem = currentItems[0];
        currentItems.RemoveAt(0);
        Destroy(leftItem);
        if (forList.Count > 0)
        {
            GameObject newItem1 = Instantiate(GetPrefabUsingForList(), content);
            currentItems.Add(newItem1);
        }

        if (forList.Count == 0)
        {
            TurnManager.Instance.NextTurn();
            GenerateForList();
        }
    }

    /// <summary>
    /// 根据 forList 获取下一个预制体。如果 forList 中有预制体的序号，则使用该数字，否则采用随机选取预制体
    /// </summary>
    /// <returns>预制体的实例化对象</returns>
    private GameObject GetPrefabUsingForList()
    {
        if (prefabPool == null || prefabPool.Count == 0)
        {
            Debug.LogWarning("预制体池为空！");
            return null;
        }
        if (forList.Count > 0)
        {
            var u=forList[0];
            int index = u.Item2;
            forList.RemoveAt(0);
            if (u.Item1 == "Player")
            {
                return prefabPool[index];
            }else if(u.Item1 == "Enemy")
            {
                return  enemyPool[index];
            }
            return ExtraItem;
        }
        else
        {
            return enditem;
        }
    }

    /// <summary>
    /// 随机选择一个预制体，作为后备选择
    /// </summary>
    /// <returns>预制体的实例化对象</returns>


    /// <summary>
    /// 自动生成下一“大回合”的 forList 序列。这里默认生成 visibleCount 个随机预制体的序号，
    /// 你也可以根据需要修改为其他逻辑，例如固定顺序或者从外部传入。
    /// </summary>
    private void GenerateForList()
    {
        forList.Clear();
        originalForList.Clear();

        foreach (Turn turn in TurnManager.Instance.CurrentTurnQueue)
        {
            if (turn.Name == "End") continue; // 跳过结束标记

            int prefabIndex = 0;
            if (turn.UnitKind == "Player" && playfab.ContainsKey(turn.Name))
                prefabIndex = playfab[turn.Name];
            else if (enemyfab.ContainsKey(turn.Name))
                prefabIndex = enemyfab[turn.Name];

            forList.Add(Tuple.Create(turn.UnitKind, prefabIndex));
        }
    }
    /// <summary>
    /// 外部调用此方法以触发“时光回溯者”的特殊逻辑。
    /// 调用后，下一个处理周期将不删除最左物体，
    /// 并将后续显示项还原为本回合开始时的序列。
    /// </summary>
    public void TriggerTimeReversal()
    {
        isTimeReversalTriggered = true;
    }
    /// <summary>
    /// 提供一个外部方法，用于调整下一周期使用的 forList 序列。可以在游戏其他逻辑中调用这个方法来指定下一个大回合的预制体顺序。
    /// </summary>
    /// <param name="newList">新的预制体序号列表</param>
    public void AdjustForList(List<Tuple<string, int>> newList)
    {
        if (newList == null)
        {
            Debug.LogWarning("新序列为 null!");
            return;
        }
        forList = new List<Tuple<string, int>>(newList);
    }
}
