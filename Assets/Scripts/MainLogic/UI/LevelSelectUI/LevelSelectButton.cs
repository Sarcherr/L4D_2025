using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainLogic.UI.LevelSelectUI
{
    public class LevelSelectButton : MonoBehaviour
    {
        /// <summary>
        /// �ؿ�ID
        /// </summary>
        public int LevelID { get; private set; }
        /// <summary>
        /// �ؿ����ƣ��������ã�
        /// </summary>
        public string LevelName { get; private set; }
        
        public Button LevelButton { get; private set; }
        
        public TextMeshProUGUI LevelNameText { get; private set; }

        private void Awake()
        {
            LevelButton = gameObject.GetComponent<Button>();
            LevelNameText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            LevelButton.onClick.AddListener(LoadLevel);
            
            // ������ťIDֱ�ӽ�ȡ��ťgameobject���Ƹ�ʽPower_ID��ĩβ��ID����
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