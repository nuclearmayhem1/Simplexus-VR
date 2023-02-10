using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public float weight = 1;
    public InteractionParamaters paramaters;
    public List<Interactor> interactors = new List<Interactor>();

    public bool canMove = false;

    public Rigidbody rb;

    private void Update()
    {
        if (canMove)
        {
            Moving();
        }
    }

    public void Moving()
    {
        Vector3 vectorSum = Vector3.zero;

        foreach (Interactor interactor in interactors)
        {
            vectorSum += interactor.controllerPosition - interactor.controls.pointOffset;
        }

        Vector3 targetPoint = (vectorSum / interactors.Count) + interactors[0].puppet.transform.position;

        //targetPoint = interactors[0].puppet.transform.rotation * targetPoint;

        rb.MovePosition(targetPoint);
        
    }



    public void Click(Interactor interactor)
    {

    }

    public void Drop(Interactor interactor)
    {
        
    }

    public void Release(Interactor interactor)
    {
        interactors.Remove(interactor);
        float totalStrenght = 0;
        foreach (Interactor interact in interactors)
        {
            totalStrenght += interact.controls.strenght;
        }

        if (totalStrenght > weight)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }

    public bool TryInteract(Interactor interactor, InteractControls controls)
    {
        Debug.Log("interacted with");
        if (paramaters.interactionType == InteractionType.pickUp)
        {
            interactor.PickUp(gameObject, paramaters);
        }
        else if (paramaters.interactionType == InteractionType.grab)
        {
            Debug.Log("yea1");
            interactors.Add(interactor);
            float totalStrenght = 0;

            foreach (Interactor interact in interactors)
            {
                totalStrenght += interact.controls.strenght;
            }

            if (totalStrenght > weight)
            {
                canMove = true;
            }
            else
            {
                canMove = false;
            }

            if (interactors.Count < 2)
            {
                interactor.Grab(gameObject, paramaters, true);
            }
            else
            {
                interactor.Grab(gameObject, paramaters, false);
            }

        }


        return true;
    }
}