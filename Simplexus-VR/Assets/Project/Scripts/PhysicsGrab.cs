using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class PhysicsGrab : MonoBehaviour
{
    public InputActionProperty inputGrab;

    public float grabDistance = 0.1f;

    public SpringSettings springSettings;

    private bool isGrabbing = false;
    private SpringJoint spring = null;

    private void OnDrawGizmos()
    {
        if (isGrabbing)
        {
            Gizmos.color = Color.Lerp(Color.green, Color.red, (spring.currentForce.magnitude / spring.breakForce));
            Gizmos.DrawLine(transform.position, spring.connectedBody.transform.position + (spring.connectedBody.transform.rotation * spring.connectedAnchor) / 3);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, grabDistance);
        }

    }

    private void Update()
    {
        if (inputGrab.action.ReadValue<float>() > 0.2f || Input.GetKeyDown(KeyCode.M))
        {
            Grab();
        }
    }

    private void Grab()
    {
        if (spring == null)
        {
            isGrabbing = false;
        }

        Debug.Log("attempted to grab");
        if (!isGrabbing)
        {
            Collider closest = Physics.OverlapSphere(transform.position, grabDistance, ~LayerMask.GetMask("Player")).Aggregate((a, b) => Vector3.Distance(transform.position, a.transform.position) < Vector3.Distance(transform.position, b.transform.position) ? a : b);

            if (closest.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Vector3 closestPoint = closest.ClosestPoint(transform.position);
                spring = gameObject.AddComponent<SpringJoint>();
                spring.connectedBody = rb;
                spring.autoConfigureConnectedAnchor = springSettings.autoConfigure;
                spring.spring = springSettings.spring;
                spring.damper = springSettings.damper;
                spring.minDistance = springSettings.minDistance;
                spring.maxDistance = springSettings.maxDistance;
                spring.tolerance = springSettings.tolerance;
                spring.breakForce = springSettings.breakForce;
                spring.breakTorque = springSettings.breakTorque;
                spring.enableCollision = springSettings.collision;
                spring.enablePreprocessing = springSettings.preproccesing;
                spring.massScale = springSettings.massScale;
                spring.connectedMassScale = springSettings.connectedMassScale;

                spring.connectedAnchor = -(rb.transform.position - closestPoint);

                isGrabbing = true;
                Debug.Log("created spring");
            }
        }
        else
        {
            Drop();
        }

    }

    private void Drop()
    {
        if (spring != null)
        {
            Destroy(spring);
            isGrabbing = false;
        }
    }

}
[System.Serializable]
public struct SpringSettings
{
    public bool autoConfigure;
    public float spring;
    public float damper;
    public float minDistance;
    public float maxDistance;
    public float tolerance;
    public float breakForce;
    public float breakTorque;
    public bool collision;
    public bool preproccesing;
    public float massScale;
    public float connectedMassScale;
}