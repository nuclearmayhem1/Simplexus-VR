using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class PuppetController : MonoBehaviour
{
    public InputActionProperty inputMove;
    public InputActionProperty inputTurn;
    public InputActionProperty inputLeftHand;
    public InputActionProperty inputRightHand;
    public InputActionProperty inputJump;

    public Transform leftHandTransform;
    public Transform rightHandTransform;

    public CharacterController character;

    public float moveSpeed = 1, turnSpeed = 1;

    private void Update()
    {
        Vector2 dirMove = inputMove.action.ReadValue<Vector2>();
        Vector2 dirTurn = inputTurn.action.ReadValue<Vector2>();
        Vector3 posLeftHand = inputLeftHand.action.ReadValue<Vector3>();
        Vector3 posRightHand = inputRightHand.action.ReadValue<Vector3>();
        bool isJumping = inputJump.action.ReadValue<bool>();

        //Move
        character.Move(dirMove * moveSpeed);
        transform.Rotate(dirTurn * turnSpeed);

        leftHandTransform.localPosition = posLeftHand;
        rightHandTransform.localPosition = posRightHand;


    }

}