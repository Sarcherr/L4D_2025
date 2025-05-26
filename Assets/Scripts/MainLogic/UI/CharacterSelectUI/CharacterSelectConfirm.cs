using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectConfirm : MonoBehaviour
{
    public Button ConfirmButton { get; private set; }

    void Start()
    {
        ConfirmButton = GetComponent<Button>();
        ConfirmButton.onClick.AddListener(Confirm);
    }

    void Confirm()
    {
        if (CharacterSelectManager.Instance.SelectedCharacters.Count < 4)
        {
            Debug.Log("Please select 4 characters.");
            return;
        }
        // 加载战斗场景
        SceneManager.LoadScene("TestLevel");
    }
}
