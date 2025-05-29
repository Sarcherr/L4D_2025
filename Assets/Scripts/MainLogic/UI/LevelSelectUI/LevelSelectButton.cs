using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainLogic.UI.LevelSelectUI
{
    public class LevelSelectButton : MonoBehaviour
    {
        /// <summary>
        /// 关卡ID
        /// </summary>
        public int LevelID { get; private set; }
        /// <summary>
        /// 关卡名称（传数据用）
        /// </summary>
        public string LevelName { get; private set; }
        
        public Button LevelButton { get; private set; }
        
        public TextMeshProUGUI LevelNameText { get; private set; }

        private void Awake()
        {
            LevelButton = gameObject.GetComponent<Button>();
            LevelNameText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            LevelButton.onClick.AddListener(LoadLevel);
            
            // 能力按钮ID直接截取按钮gameobject名称格式Power_ID中末尾的ID数字
            string[] name = gameObject.name.Split('_');
            if (name.Length > 1)
            {
                LevelID = int.Parse(name[1]);
            }
            else
            {
                Debug.LogError("LevelSelectButton name format error, please check the name format.");
            }
        
        
            UIManager.Instance.RegisterUI("LevelSelectUI.LevelSelectButton",LevelButton);
            
        }

        public void Refresh(string levelName)
        {
            LevelName = levelName;
            LevelNameText.text = levelName; 
            // Debug.Log(levelName);
        }

        public void LoadLevel()
        {
            LevelSelectManager.Instance.SetMonsterNames(LevelName);
            // Debug.Log(LevelSelectManager.Instance.MonsterNames[0]);

            SceneManager.LoadScene("TestLevel");
        }
    }
}