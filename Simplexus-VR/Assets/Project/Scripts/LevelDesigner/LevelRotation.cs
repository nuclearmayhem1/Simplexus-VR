using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LevelRotation : MonoBehaviour
{
    public XRGrabInteractable XRGrab;
    bool rotating = false;
    public Quaternion grabRotation = Quaternion.identity;
    public Quaternion currentRotation = Quaternion.identity;
    public Quaternion deltaRotation = Quaternion.identity;

    private void Start()
    {
        currentRotation = transform.rotation;
    }

    public void Rotate(bool activate)
    {
        rotating = activate;

        if (activate)
        {
            grabRotation = Quaternion.LookRotation(transform.position - XRGrab.firstInteractorSelecting.transform.position);
        }
        else
        {
            currentRotation = deltaRotation * currentRotation;
        }

    }

    private void Update()
    {
        if (rotating)
        {
            deltaRotation = Quaternion.Inverse(grabRotation) * Quaternion.LookRotation(transform.position - XRGrab.firstInteractorSelecting.transform.position);
            transform.rotation = currentRotation * deltaRotation;
        }
    }

}
