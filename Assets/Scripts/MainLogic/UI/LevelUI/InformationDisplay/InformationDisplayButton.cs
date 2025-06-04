using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationDisplayButton : MonoBehaviour
{
    public Button Button { get; private set; }
    public UnitData UnitData { get; private set; }
    public TextMeshProUGUI InformationText { get; private set; }
    public GameObject InformationPanel { get; private set; }
    
    private void Awake()
    {
        Button = GetComponent<Button>();
        InformationText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        InformationPanel = transform.GetChild(1).gameObject;
        InformationPanel.SetActive(false);

        if (InformationPanel == null)
        {
            Debug.LogError("InformationPanel is not assigned in " + gameObject.name);
        }

        UIManager.Instance.RegisterUI("LevelUI.InformationDisplay.InformationDisplayButton", Button);
    }

    public void Refresh(UnitData data)
    {
        UnitData = data;
        if (UnitData != null)
        {
            // todo: 根据UnitData刷新信息文本
            //InformationText.text = UnitData.;
        }
        else
        {
            InformationText.text = "No Data";
        }
        
        Button.onClick.AddListener(DisplayInformation);
    }
    

    private void DisplayInformation()
    {
        InformationPanel.SetActive(true);
        // todo: 显示信息面板内容
    }
    
}
