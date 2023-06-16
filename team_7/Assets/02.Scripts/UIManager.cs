using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject Button_Start;
    public GameObject Button_HowTo;
    public GameObject PanelHowTo;
    public GameObject Button_Back;

    void Start()
    {
        PanelHowTo.SetActive(false);

    }

    public void ButtonStart_Clicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ButtonHowTo_Clicked()
    {
        PanelHowTo.SetActive(true);
    }

    public void ButtonBack_Clicked()
    {
        PanelHowTo.SetActive(false);
    }
}
