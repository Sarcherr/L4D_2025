using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.LevelUI.Controller
{
    /// <summary>
    /// 技能按钮
    /// </summary>
    public class PowerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string PowerName { get; private set; }
        public string PowerDescription { get; private set; }
        public int PowerID { get; private set; }
        public Button Button { get; private set; }
        public TextMeshProUGUI Text { get; private set; }
        /// <summary>
        /// 悬浮窗object
        /// </summary>
        public GameObject DescriptionWindow { get; private set; }
        public TextMeshProUGUI DescriptionText { get; private set; }
        public bool isFollowMouse = true;

        private void Awake()
        {
            Button = GetComponent<Button>();
            Text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            DescriptionWindow = transform.GetChild(1).gameObject;
            DescriptionText = DescriptionWindow.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            DescriptionWindow.SetActive(false);
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
        public void Refresh(string name, string description = null)
        {
            PowerName = name;
            Text.text = name;
            PowerDescription = description;
        }

        #region 悬浮窗相关
        public void OnPointerEnter(PointerEventData eventData)
        {
            RefreshDescription();
            DescriptionWindow.SetActive(true);
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DescriptionWindow.SetActive(false);
        }

        public void RefreshDescription()
        {
            DescriptionText.text = PowerDescription;
        }

        #endregion

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        public void Power()
        {
            Debug.Log($"{PowerID}: {PowerName}");
            if (PowerName != null && UIManager.Instance.IsCurrentUnit)
            {
                ControllerManager.Instance.Player.Power(PowerName);
            }
        }
    }
}
