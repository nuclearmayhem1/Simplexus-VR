using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp_Component : MonoBehaviour
{
    public Light lamp;

    public void TurnOn()
    {
        lamp.enabled = true;
    }
    public void TurnOff()
    {
        lamp.enabled = false;
    }

    public void Toggle(bool value)
    {
        lamp.enabled = value;
    }
}
