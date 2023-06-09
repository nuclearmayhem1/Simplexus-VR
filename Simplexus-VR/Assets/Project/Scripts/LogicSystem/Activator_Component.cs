using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activator_Component : MonoBehaviour
{
    public UnityEvent on = new UnityEvent();
    public UnityEvent off = new UnityEvent();

    public void Toggle(bool value)
    {
        if (value)
        {
            on.Invoke();
        }
        else
        {
            off.Invoke();
        }
    }
}
