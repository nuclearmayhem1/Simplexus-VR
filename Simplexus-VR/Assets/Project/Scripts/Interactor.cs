using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float strenght = 1;
    public Transform grabPoint;
    public bool interacting = false;
    public bool grabbing = false;
    public GameObject interactable;

    public void pickUp(GameObject interacted)
    {

    }

    public void drop()
    {

    }

}

public struct InteractionParamaters
{
    public InteractionType interactionType;
    public Vector3 offset;
    public bool isGrabbing;
}

public enum InteractionType
{
    interact, pickUp, grab
}