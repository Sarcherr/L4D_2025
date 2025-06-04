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

    [Header("Ԥ��������")]
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
  
    // ��ʶ�Ƿ񴥷��ˡ�ʱ����ݡ������߼�
    private bool isTimeReversalTriggered = false;

    void Awake()
    {
        if (contentobj != null)
        {
            content = contentobj.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("contentobj û�и�ֵ��");
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
    /// ��ѭ����ÿ��һ��ʱ�䴦��һ���Ƴ�����ߵ����壬���� forList ��ȡ��һ���µ�����������Ҳ�
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
    /// ������������Ƴ����Ҳ������������߼�
    /// </summary>
    private void HandleLeftOutAndRightIn()
    {
        if (TurnManager.Instance.CurrentTurn.Name == "End")
        {
            //����ÿ���غϽ���֮���ego��Ӻ������غϽ���Ч��
            return;
        }
        if (currentItems.Count == 0) return;
        if (isTimeReversalTriggered)
        {
            // ������������壬���������ٴ���
            GameObject nleftItem = currentItems[0];
            Vector3 pos = nleftItem.transform.localPosition;
            Transform parent = nleftItem.transform.parent;
            Destroy(nleftItem);
            currentItems.RemoveAt(0);
            GameObject extrai = Instantiate(ExtraItem, parent);
            extrai.transform.localPosition = pos;
            currentItems.Insert(0, extrai);

            // ���������ʾ�����壨���ٲ����б����Ƴ���
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
        // �Ƴ�����������
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
    /// ���� forList ��ȡ��һ��Ԥ���塣��� forList ����Ԥ�������ţ���ʹ�ø����֣�����������ѡȡԤ����
    /// </summary>
    /// <returns>Ԥ�����ʵ��������</returns>
    private GameObject GetPrefabUsingForList()
    {
        if (prefabPool == null || prefabPool.Count == 0)
        {
            Debug.LogWarning("Ԥ�����Ϊ�գ�");
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
    /// ���ѡ��һ��Ԥ���壬��Ϊ��ѡ��
    /// </summary>
    /// <returns>Ԥ�����ʵ��������</returns>


    /// <summary>
    /// �Զ�������һ����غϡ��� forList ���С�����Ĭ������ visibleCount �����Ԥ�������ţ�
    /// ��Ҳ���Ը�����Ҫ�޸�Ϊ�����߼�������̶�˳����ߴ��ⲿ���롣
    /// </summary>
    private void GenerateForList()
    {
        forList.Clear();
        originalForList.Clear();

        foreach (Turn turn in TurnManager.Instance.CurrentTurnQueue)
        {
            if (turn.Name == "End") continue; // �����������

            int prefabIndex = 0;
            if (turn.UnitKind == "Player" && playfab.ContainsKey(turn.Name))
                prefabIndex = playfab[turn.Name];
            else if (enemyfab.ContainsKey(turn.Name))
                prefabIndex = enemyfab[turn.Name];

            forList.Add(Tuple.Create(turn.UnitKind, prefabIndex));
        }
    }
    /// <summary>
    /// �ⲿ���ô˷����Դ�����ʱ������ߡ��������߼���
    /// ���ú���һ���������ڽ���ɾ���������壬
    /// ����������ʾ�ԭΪ���غϿ�ʼʱ�����С�
    /// </summary>
    public void TriggerTimeReversal()
    {
        isTimeReversalTriggered = true;
    }
    /// <summary>
    /// �ṩһ���ⲿ���������ڵ�����һ����ʹ�õ� forList ���С���������Ϸ�����߼��е������������ָ����һ����غϵ�Ԥ����˳��
    /// </summary>
    /// <param name="newList">�µ�Ԥ��������б�</param>
    public void AdjustForList(List<Tuple<string, int>> newList)
    {
        if (newList == null)
        {
            Debug.LogWarning("������Ϊ null!");
            return;
        }
        forList = new List<Tuple<string, int>>(newList);
    }
}
