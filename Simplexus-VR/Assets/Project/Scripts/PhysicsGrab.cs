using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsGrab : MonoBehaviour
{
    public InputActionProperty inputGrab;

    private SpringJoint spring;
    private bool isGrabbing = false;

    private void Update()
    {
        if (inputGrab.action.WasPressedThisFrame())
        {
            Grab();
        }
    }

    public void Grab()
    {

        if (!isGrabbing)
        {
            Physics.OverlapSphere();
        }


    }

}
