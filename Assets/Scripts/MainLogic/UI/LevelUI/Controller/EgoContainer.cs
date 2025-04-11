using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
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
        /// Ego槽列表
        /// </summary>
        public List<EgoItem> EgoItems { get; private set; } = new List<EgoItem>();
        /// <summary>
        /// Ego单元槽
        /// </summary>
        public EgoItem EgoItemPrefab;

        // todo: Ego槽的刷新
        // todo: Ego槽的特定规则选取
    }
}
