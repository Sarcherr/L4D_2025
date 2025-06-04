using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        /// <summary>
        /// 能否进行交互
        /// </summary>
        public bool IsActive { get; private set; }
        /// <summary>
        /// button组件，用于交互
        /// </summary>
        public Button EgoButton { get; private set; }
        /// <summary>
        /// ego名称
        /// </summary>
        public TextMeshProUGUI EgoName { get; private set; }
        /// <summary>
        /// ego ID
        /// </summary>
        public int EgoID { get; private set; }


        private void Awake()
        {
            EgoButton = GetComponent<Button>();
            EgoName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            
            string[] name = gameObject.name.Split('_');
            if (name.Length > 1)
            {
                EgoID = int.Parse(name[1]);
            }
            else
            {
                Debug.LogError("EgoItem name format error, please check the name format.");
            }


            UIManager.Instance.RegisterUI("LevelUI.Controller.EgoItem", EgoButton);
        }
        
        public void SetItemEgo(Ego ego)
        {
            ItemEgo = ego;
            
        }

        // todo: Ego槽的刷新
        public void Refresh()
        {
            EgoName.text = ItemEgo.EgoType;

            EgoButton.onClick.RemoveAllListeners();
            EgoButton.onClick.AddListener(Activate);
        }

        public void Activate()
        {
            
        }
        
        // todo: Ego槽的特定规则选取
    }

}
