using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever_Component : MonoBehaviour, IInteractable
{
    public UnityEvent<bool> onSwitch = new UnityEvent<bool>();
    public UnityEvent off = new UnityEvent();
    public UnityEvent on = new UnityEvent();

    private bool state = false;

    public void Interact()
    {
        state = !state;
        onSwitch.Invoke(state);
        if (state)
        {
            on.Invoke();
        }
        else
        {
            off.Invoke();
        }
    }
}
