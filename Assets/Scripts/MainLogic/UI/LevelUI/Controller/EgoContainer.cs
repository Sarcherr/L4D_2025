using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LevelUI.Controller
{
    public class EgoContainer : MonoBehaviour
    {
        /// <summary>
        /// Ego容量
        /// </summary>
        public int Capacity { get; private set; }
        /// <summary>
        /// Ego白条值(情感爆发阈值)
        /// </summary>
        public int Threshold { get; private set; }
        /// <summary>
        /// 目标Ego槽列表
        /// </summary>
        public List<Ego> TargetEgos { get; private set; }
        /// <summary>
        /// 当前Ego槽列表
        /// </summary>
        public List<EgoItem> CurrentEgoItems { get; private set; } = new List<EgoItem>();
        /// <summary>
        /// Ego单元槽
        /// </summary>
        public EgoItem EgoItem{ get; private set; }
        /// <summary>
        /// Ego槽预制体
        /// </summary>
        public GameObject EgoItemPrefab { get; private set; }
        /// <summary>
        /// Ego槽容器Transform（即为本身的Transform）
        /// </summary>
        public Transform EgoContainerTransform { get; private set; }

        private void Awake()
        {
            EgoItemPrefab = Resources.Load<GameObject>("Prefabs/EgoItem");
            EgoContainerTransform = transform;
            
            UIManager.Instance.RegisterEgoContainer(this);
        }
        
        //注意此函数尚未被调用过
        public void RefreshEgoItems(List<Ego> egos)
        {
            CurrentEgoItems.Clear();
            int egoIndex = 0;
            foreach (Ego ego in egos)
            {
                GameObject go = Instantiate(EgoItemPrefab, EgoContainerTransform);
                go.transform.SetParent(EgoContainerTransform);
                go.name = "EgoItem_" + egoIndex;
                go.GetComponent<EgoItem>().SetItemEgo(ego);
                go.GetComponent<EgoItem>().Refresh();
                CurrentEgoItems.Add(go.GetComponent<EgoItem>());
                egoIndex++;
            }
        }
        
        // todo: Ego槽的特定规则选取
    }


}
