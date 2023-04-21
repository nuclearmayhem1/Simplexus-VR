using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class InteractionManager : MonoBehaviour
{
    public float radius = 0.1f;

    public InputActionProperty interactKey;

    private void Update()
    {
        if (interactKey.action.IsPressed())
        {
            Interact();
        }
    }

    private void Interact()
    {
        IInteractable closestInteractable = null;
        float distance = float.MaxValue;

        foreach (Collider col in Physics.OverlapSphere(transform.position, radius))
        {
            if (col.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                float newDistance = Vector3.Distance(transform.position, col.transform.position);

                if (newDistance < distance)
                {
                    distance = newDistance;
                    closestInteractable = interactable;
                }
            }
        }

        if (closestInteractable!= null)
        {
            closestInteractable.Interact();
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
