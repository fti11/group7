using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GAMESTATE
    {
        GAMEINIT,
        GAMESTART,
        GAMEPLAY,
        GAMEEND
    }

    public GAMESTATE gameState = GAMESTATE.GAMEINIT;

    void Start()
    {
        if (gameState == GAMESTATE.GAMEINIT)    //�÷��̾�1�� ������ �� ����
        {
           
            gameState = GAMESTATE.GAMESTART;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

}
