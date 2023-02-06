using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool TryInteract(Interactor interactor, InteractControls parameters);
    public void Drop(Interactor interactor);
    public void Click(Interactor interactor);
    public void Release(Interactor interactor);
}