using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Interactor : MonoBehaviour
{
    public Transform grabPoint;
    public bool holding = false;
    public bool grabbing = false;
    public bool primary = false;
    public bool twoHanded = false;
    public GameObject interactableObject = null;
    public IInteractable interactable = null;

    public float radius = 0.1f;

    public InteractControls controls;

    public Transform handTransform;
    public Transform IKTargetTransform;

    public float handDeviation { get { return Vector3.Distance(handTransform.position, IKTargetTransform.position); } }
    public float maxHandDeviation = 0.2f;

    public PuppetController puppet;

    public Vector3 controllerPosition = Vector3.zero;
    public float controllerDeviation { get { return Vector3.Distance(controllerPosition, transform.position); } }
    public float maxControllerDeviation = 0.2f;

    public Hand hand;

    public void CalculateHandOffset()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(grabPoint.position, radius);
    }

    private void Update()
    {
        if (hand == Hand.left)
        {
            controllerPosition = puppet.posLeftHand;
        }
        else if (hand == Hand.right)
        {
            controllerPosition = puppet.posRightHand;
        }

        if (grabbing && handDeviation > maxHandDeviation)
        {
            //Release();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("grab");
            TryInteract();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("release");
            Release();
        }

        if (grabbing)
        {
            WhenGrabbing();
        }

    }

    public void WhenGrabbing()
    {
        IKTargetTransform.position = interactableObject.transform.position - (interactableObject.transform.rotation * controls.pointOffset);
    }

    public void TryInteract()
    {
        Collider[] colliders = Physics.OverlapSphere(grabPoint.position, radius);

        Transform closestTransfrom = null;
        Collider closestCollider = null;
        IInteractable closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.name);
            if (collider.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                float distance = Vector3.Distance(collider.gameObject.transform.position, grabPoint.position);
                Debug.Log(distance);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                    closestTransfrom = collider.transform;
                    closestCollider = collider;
                }
            }
        }
        if (closestInteractable != null)
        {
            Debug.Log(closestTransfrom.gameObject.name);
            interactableObject = closestTransfrom.gameObject;
            controls.closestPoint = closestCollider.ClosestPoint(grabPoint.position);
            controls.pointOffset = closestTransfrom.position - controls.closestPoint;
            interactable = closestInteractable;
            closestInteractable.TryInteract(this, controls);
        }
    }

    public void Drop()
    {
        if (interactableObject != null)
        {
            interactable.Drop(this);
            interactableObject.transform.parent = null;
            interactableObject = null;
            interactable = null;
            holding = false;
            IKTargetTransform.localPosition = Vector3.zero;
        }
    }

    public void Release()
    {
        if (interactableObject != null)
        {
            interactable.Release(this);
            grabbing = false;
            interactableObject = null;
            interactable = null;
            IKTargetTransform.localPosition = Vector3.zero;
            if (hand == Hand.left)
            {
                puppet.overrideLeftHand = false;
            }
            else if (hand == Hand.right)
            {
                puppet.overrideRightHand = false;
            }
        }
    }

    public void PickUp(GameObject target, InteractionParamaters parameters)
    {
        if (controls.pickUpType == PickUpType.instant)
        {
            interactableObject = target;
            interactableObject.transform.parent = grabPoint;
            holding = true;
        }
    }

    public void Grab(GameObject target, InteractionParamaters paramaters, bool isPrimary)
    {
        grabbing = true;

        if (hand == Hand.right)
        {
            puppet.overrideRightHand = true;
        }
        else if (hand == Hand.left)
        {
            puppet.overrideLeftHand = true;
        }

    }

}
[System.Serializable]
public struct InteractionParamaters
{
    public InteractionType interactionType;
    public Vector3 offset;
}
[System.Serializable]
public struct InteractControls
{
    public float strenght;
    public PickUpType pickUpType;
    public Vector3 closestPoint;
    public Vector3 pointOffset;
}

public enum InteractionType
{
    interact, pickUp, grab
}

public enum PickUpType
{
    instant, interpolated, physics
}

public enum Hand
{
    left, right
}