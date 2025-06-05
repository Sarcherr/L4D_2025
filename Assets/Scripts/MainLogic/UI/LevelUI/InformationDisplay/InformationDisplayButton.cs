using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationDisplayButton : MonoBehaviour
{
    public Button Button { get; private set; }
    public RuntimeUnitData UnitData { get; private set; }
    public TextMeshProUGUI InformationText { get; private set; }
    public GameObject InformationPanel { get; private set; }
    
    private void Awake()
    {
        Button = GetComponent<Button>();
        InformationPanel = transform.GetChild(1).gameObject;
        InformationText = InformationPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        InformationPanel.SetActive(false);

        if (InformationPanel == null)
        {
            Debug.LogError("InformationPanel is not assigned in " + gameObject.name);
        }

        UIManager.Instance.RegisterUI("LevelUI.InformationDisplay.InformationDisplayButton", Button);
    }

    public void Refresh(RuntimeUnitData data)
    {
        UnitData = data;
        if (UnitData != null)
        {
            // todo: 根据UnitData刷新信息文本
            InformationText.text = UnitData.Name +"\n" +
                                   "HP: " + UnitData.CurrentHealth + "\n" +
                                   "Attack: " + UnitData.CurrentAttack + "\n";
        }
        else
        {
            InformationText.text = "No Data";
        }
        
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(DisplayInformation);
    }
    

    private void DisplayInformation()
    {
        Debug.Log("DisplayInformation called for " + UnitData.Name);
        if (!InformationPanel.activeInHierarchy)
        {
            InformationPanel.SetActive(true);
        }
        else
        {
            InformationPanel.SetActive(false);
        }
        
        
    }
    
}
