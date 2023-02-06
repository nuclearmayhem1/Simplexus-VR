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
    public GameObject interactableObject;
    public IInteractable interactable = null;

    public float radius = 0.1f;

    public InteractControls controls;

    public Transform handTransform;
    public Transform IKTargetTransform;

    public float handDeviation { get { return Vector3.Distance(handTransform.position, IKTargetTransform.position); } }
    public float maxHandDeviation = 0.2f;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(grabPoint.position, radius);
    }

    private void Update()
    {
        if (grabbing && handDeviation > maxHandDeviation)
        {
            Release();
        }
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
            if (TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                float distance = Vector3.Distance(collider.gameObject.transform.position, grabPoint.position);
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
            closestInteractable.TryInteract(this, controls);
            controls.closestPoint = closestCollider.ClosestPoint(grabPoint.position);
            interactable = closestInteractable;
        }
    }

    public void Drop()
    {
        if (interactableObject != null)
        {
            
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
            grabbing = false;
            interactableObject = null;
            interactable = null;
            IKTargetTransform.localPosition = Vector3.zero;
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
}

public enum InteractionType
{
    interact, pickUp, grab
}

public enum PickUpType
{
    instant, interpolated, physics
}