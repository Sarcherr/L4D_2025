using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    /// <summary>
    /// 技能按钮
    /// </summary>
    public class SkillButton : MonoBehaviour
    { 
        public string SkillName { get; private set; }
        public Button Button { get; private set; }
        public TextMeshPro Text { get; private set; }

        private void Start()
        {
            Button = GetComponent<Button>();
            Text = GetComponentInChildren<TextMeshPro>();
            Button.onClick.AddListener(Power);
        }

        public void Refresh(string name)
        {
            SkillName = name;
            Text.text = name;
        }

        public void Power()
        {
            Debug.Log(SkillName);
            if(SkillName != "null")
            {
                ControllerManager.Instance.Player.Power(SkillName);
            }
        }
    }
}
