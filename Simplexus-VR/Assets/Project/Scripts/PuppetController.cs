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

    public float gravity = -1;

    public float handDistanceMultiplier = 2;

    private void Update()
    {
        Vector2 dirMove = inputMove.action.ReadValue<Vector2>();
        Vector2 dirTurn = inputTurn.action.ReadValue<Vector2>();
        Vector3 posLeftHand = inputLeftHand.action.ReadValue<Vector3>();
        Vector3 posRightHand = inputRightHand.action.ReadValue<Vector3>();
        bool isJumping = inputJump.action.ReadValue<bool>();

        //Move
        Vector3 moveVector = new Vector3(dirMove.x, isJumping? -gravity * 2 * Time.deltaTime : 0 + (gravity * Time.deltaTime), dirMove.y);
        Vector3 turnVector = new Vector3(0, dirTurn.x * Time.deltaTime, 0);

        

        character.Move(transform.rotation * moveVector * moveSpeed);
        transform.Rotate(turnVector * turnSpeed);

        leftHandTransform.localPosition = transform.rotation * posLeftHand * handDistanceMultiplier;
        rightHandTransform.localPosition = transform.rotation * posRightHand * handDistanceMultiplier;


    }

}