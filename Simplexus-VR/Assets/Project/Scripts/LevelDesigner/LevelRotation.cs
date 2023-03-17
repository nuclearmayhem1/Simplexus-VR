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
    public Vector3 grabVector;

    private void Start()
    {
        currentRotation = transform.rotation;
    }

    public void Rotate(bool activate)
    {
        rotating = activate;

        if (activate)
        {
            grabVector = transform.position - XRGrab.firstInteractorSelecting.transform.position;
            //grabRotation = Quaternion.LookRotation(transform.position - XRGrab.firstInteractorSelecting.transform.position);
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
            var newVector = transform.position - XRGrab.firstInteractorSelecting.transform.position;
            deltaRotation = Quaternion.FromToRotation(grabVector, newVector);
            transform.rotation = currentRotation * deltaRotation;
        }
    }

}
