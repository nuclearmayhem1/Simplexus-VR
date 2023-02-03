using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class SimpleInteraction : MonoBehaviour, IInteractable
{
    public void Drop(Interactor interactor)
    {
        throw new NotImplementedException();
    }

    public bool TryInteract(Interactor interactor)
    {
        interactor.pickUp(gameObject);
        return true;
    }
}