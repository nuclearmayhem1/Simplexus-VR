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

    public void Click(Interactor interactor)
    {
        
    }

    public void Drop(Interactor interactor)
    {
        
    }

    public void Release(Interactor interactor)
    {
        interactors.Remove(interactor);
    }

    public bool TryInteract(Interactor interactor, InteractControls controls)
    {
        if (paramaters.interactionType == InteractionType.pickUp)
        {
            interactor.PickUp(gameObject, paramaters);
        }
        else if (paramaters.interactionType == InteractionType.grab)
        {
            interactors.Add(interactor);

            float totalStrenght = 0;

            foreach (Interactor interact in interactors)
            {
                totalStrenght += interact.controls.strenght;
            }

            if (totalStrenght > weight)
            {
                bool first = true;
                foreach (Interactor interact in interactors)
                {
                    if (first)
                    {
                        interact.Grab(gameObject, paramaters, true);
                        first = false;
                    }
                    else
                    {
                        interact.Grab(gameObject, paramaters, false);
                    }
                }
            }

        }


        return true;
    }

}