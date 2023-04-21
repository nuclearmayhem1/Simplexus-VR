using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleInteractable : MonoBehaviour, IInteractable
{
    public UnityEvent on;
    public UnityEvent off;
    public bool state = false;

    public void Interact()
    {
        state = !state;
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
