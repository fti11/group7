using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMenuUI : MonoBehaviour
{
    public Button buttonStart;
    public Button buttonExit;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        buttonStart.onClick.AddListener(OnClickButtonStart);
        buttonExit.onClick.AddListener(OnClickButtonExit);
    }

    private void OnClickButtonStart()
    {
        SceneManager.LoadScene("03.GameScene");
    }

    private void OnClickButtonExit()
    {
        Application.Quit();
    }
}
