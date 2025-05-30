using UnityEngine;

namespace UI.LevelUI.Controller
{
    public class EgoItem : MonoBehaviour
    {
        /// <summary>
        /// 是否为情感爆发Ego槽
        /// </summary>
        public bool isBurstEgo = false;
        /// <summary>
        /// Ego槽容纳Ego
        /// </summary>
        public Ego ItemEgo { get; private set; }

        // todo: Ego槽的刷新
        // todo: Ego槽的特定规则选取
    }
}
