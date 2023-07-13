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
        if (gameState == GAMESTATE.GAMEINIT)    //플레이어1을 시작할 때 생성
        {
           
            gameState = GAMESTATE.GAMESTART;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

}
