using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationActions : MonoBehaviour
{
    [HideInInspector] public AnimationController animationController;

    public void Awake()
    {
        animationController = GetComponent<AnimationController>();
    }

    public void TakeAction(string action)
    {
        if (action == "Idle")
        {
            animationController.TriggerAnimation("Idle");
            animationController.ChangeCharacterState(0.0f, AnimationsState.Idle);
        }
        if (action == "TurnLeft")
        {
            animationController.TriggerAnimation("TurnLeft");
            animationController.ChangeCharacterState(0.0f, AnimationsState.Idle);
        }
        if (action == "TurnRight")
        {
            animationController.TriggerAnimation("TurnRight");
            animationController.ChangeCharacterState(0.0f, AnimationsState.Idle);
        }

        if (action == "Damage")
        {
            animationController.TriggerAnimation("Damage");
            animationController.ChangeCharacterState(0.1f, AnimationsState.Idle);           
        }

        if (action == "Attack")
        {
            animationController.TriggerAnimation("Attack");
            animationController.ChangeCharacterState(0.1f, AnimationsState.Idle);          
        }

        if (action == "Run")
        {
            animationController.TriggerAnimation("Run");
            animationController.ChangeCharacterState(0.0f, AnimationsState.move);           
        }

        if (action == "RunAttack")
        {
            animationController.TriggerAnimation("RunAttack");
            animationController.ChangeCharacterState(0.1f, AnimationsState.move);
        }

        if (action == "RunBack")
        {
            animationController.TriggerAnimation("RunBack");
            animationController.ChangeCharacterState(0.1f, AnimationsState.move);
        }
    }
}