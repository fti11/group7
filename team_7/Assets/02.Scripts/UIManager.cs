using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject Button_Start;
    public GameObject Button_Quit;

    public void ButtonStart_Clicked()
    {
        SceneManager.LoadScene("02.Classroom");
    }

    public void ButtonQuit_Clicked()
    {
        Application.Quit();
    }
}
