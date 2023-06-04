using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationsState
{
    Idle,
    move
}

public enum AnimatorTriggers
{
    Idle = 0,
    TurnLeft = 1,
    TurnRight = 2,
    Damage = 10,
    Attack = 20,
    Run = 30,
    RunAttack = 31,
    RunBack = 40,
}