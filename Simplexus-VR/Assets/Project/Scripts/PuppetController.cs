using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class PuppetController : MonoBehaviour
{
    public InputActionProperty inputMove;
    public InputActionProperty inputTurn;
    public InputActionProperty inputLeftHandPos;
    public InputActionProperty inputRightHandPos;
    public InputActionProperty inputLeftHandRot;
    public InputActionProperty inputRightHandRot;
    public InputActionProperty inputJump;

    public Transform leftHandTransform;
    public Transform rightHandTransform;

    public float moveSpeed = 1, turnSpeed = 1;

    public float handDistanceMultiplier = 2;

    public Quaternion leftHandOffset;
    public Quaternion rightHandOffset;

    public bool overrideLeftHand = false;
    public bool overrideRightHand = false;

    public Vector3 posLeftHand = Vector3.zero;
    public Vector3 posRightHand = Vector3.zero;
    public Quaternion rotLeftHand = Quaternion.identity;
    public Quaternion rotRightHand = Quaternion.identity;

    public Level level;

    public float gravity = 1;

    public Rigidbody rb;


    private void FixedUpdate()
    {
        Vector2 dirMove = inputMove.action.ReadValue<Vector2>();
        Vector2 dirTurn = inputTurn.action.ReadValue<Vector2>();
        posLeftHand = inputLeftHandPos.action.ReadValue<Vector3>();
        posRightHand = inputRightHandPos.action.ReadValue<Vector3>();
        rotLeftHand = inputLeftHandRot.action.ReadValue<Quaternion>();
        rotRightHand = inputRightHandRot.action.ReadValue<Quaternion>();
        //bool isJumping = inputJump.action.ReadValue<bool>();

        //Move

        Vector3 upDirection = (transform.position - level.transform.position).normalized;

        transform.rotation = Quaternion.FromToRotation(transform.up, upDirection) * transform.rotation;
        transform.RotateAround(transform.up, dirTurn.x * Time.deltaTime);

        rb.AddForce(upDirection * gravity * Time.deltaTime);
        rb.AddRelativeForce(new Vector3(dirMove.x, 0, dirMove.y) * moveSpeed, ForceMode.Acceleration);


        if (!overrideLeftHand)
        {
            leftHandTransform.localPosition = posLeftHand * handDistanceMultiplier;
            leftHandTransform.localRotation = rotLeftHand * leftHandOffset;
        }

        if (!overrideRightHand)
        {
            rightHandTransform.localRotation = rotRightHand * rightHandOffset;
            rightHandTransform.localPosition = posRightHand * handDistanceMultiplier;
        }

    }

}