using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.LevelUI.Controller
{
    /// <summary>
    /// 技能按钮
    /// </summary>
    public class PowerButton : MonoBehaviour
    { 
        public string PowerName { get; private set; }
        public int PowerID { get; private set; }
        public Button Button { get; private set; }
        public TextMeshProUGUI Text { get; private set; }

        private void Start()
        {
            Button = GetComponent<Button>();
            Text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            Button.onClick.AddListener(Power);

            // 能力按钮ID直接截取按钮gameobject名称格式Power_ID中末尾的ID数字
            string[] name = gameObject.name.Split('_');
            if (name.Length > 1)
            {
                PowerID = int.Parse(name[1]);
            }
            else
            {
                Debug.LogError("PowerButton name format error, please check the name format.");
            }

            // 注册UI
            UIManager.Instance.RegisterUI("LevelUI.Controller.PowerButton", Button);
        }

        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="name">对应能力名称</param>
        public void Refresh(string name)
        {
            PowerName = name;
            Text.text = name;
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        public void Power()
        {
            Debug.Log($"{PowerID}: {PowerName}");
            if(PowerName != null)
            {
                ControllerManager.Instance.Player.Power(PowerName);
            }
        }
    }
}
