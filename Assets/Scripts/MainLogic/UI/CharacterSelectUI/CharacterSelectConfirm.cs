using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectConfirm : MonoBehaviour
{
    public Button ConfirmButton{get; private set;}

    void Start()
    {
        ConfirmButton = GetComponent<Button>();
        ConfirmButton.onClick.AddListener(Confirm);
    }

    void Confirm()
    {
        SceneManager.LoadScene("TestLevel");
    }
}
