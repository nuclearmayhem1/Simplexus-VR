using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class InteractionManager : MonoBehaviour
{
    public float radius = 0.1f;
    public float interactionCooldown = 1f;
    private float interactionCooldownCurrent = 0;

    public InputActionProperty interactKey;


    private void Update()
    {
        if (interactionCooldownCurrent > 0)
        {
            interactionCooldownCurrent -= Time.deltaTime;
        }
        
        if (interactKey.action.WasPressedThisFrame() && interactionCooldownCurrent <= 0)
        {
            interactionCooldownCurrent = interactionCooldown;
            Interact();
        }
    }

    private void Interact()
    {
        IInteractable closestInteractable = null;
        float distance = float.MaxValue;

        foreach (Collider col in Physics.OverlapSphere(transform.position, radius))
        {
            if (col.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
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
