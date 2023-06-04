using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    private int enumLength;
    private int initialGamepadCount;

    private void Start()
    {
        enumLength = Enum.GetValues(typeof(KeyMap.KEYTYPE)).Length;

        string[] connectedGamepads = Input.GetJoystickNames();
        int currentGamepadCount = connectedGamepads.Length;
        for (int i = initialGamepadCount; i < currentGamepadCount; i++)
        {
            Debug.Log("New Gamepad " + (i + 1) + " Name: " + connectedGamepads[i]);
        }

    }

    private void Update()       //입력 값이 뭔지 찾아내기 위해 적어놓은것같아용
    {
        for (int i = 0; i < enumLength; i++)
        {
            string enumString = Enum.GetName(typeof(KeyMap.KEYTYPE), i);

            if (Input.GetButtonDown(enumString))
            {
                Debug.Log(enumString + " Key is down");
            }
        }
    }
}
