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

    public GameObject[] player = new GameObject[2];
    public GameObject[] playerPrefabs = new GameObject[2];

    public Transform[] playerStartPoint = new Transform[2];
    // Start is called before the first frame update
    void Start()
    {
        if (gameState == GAMESTATE.GAMEINIT)    //플레이어1을 시작할 때 생성
        {
            if (player[0] == null)
            {

                GameObject temp = Instantiate(playerPrefabs[0]);
                player[0] = temp;
                temp.transform.position = playerStartPoint[0].position;

            }
            gameState = GAMESTATE.GAMESTART;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerGen();
    }

    void PlayerGen()            //F1과 F2로 플레이어들을 생성
    {
        if (gameState > GAMESTATE.GAMEINIT)
        {
            if (Input.GetButtonDown("GamePad1_START") || Input.GetButtonDown("GamePad1_RT") || Input.GetKeyDown(KeyCode.F1))
            {
                if (player[0] == null)
                {
                    GameObject temp = Instantiate(playerPrefabs[0]);
                    player[0] = temp;
                    temp.transform.position = playerStartPoint[0].position;
                }
            }

            if (Input.GetButtonDown("GamePad2_START") || Input.GetButtonDown("GamePad2_RT") || Input.GetKeyDown(KeyCode.F2))
            {
                if (player[1] == null)
                {
                    GameObject temp = Instantiate(playerPrefabs[1]);
                    player[1] = temp;
                    temp.transform.position = playerStartPoint[1].position;
                }
            }


        }
    }
}
