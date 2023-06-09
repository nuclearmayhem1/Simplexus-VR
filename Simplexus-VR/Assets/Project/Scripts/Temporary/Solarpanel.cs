using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Solarpanel : MonoBehaviour
{
    public bool state = false;
    public Transform targetSun;

    public UnityEvent<bool> output = new UnityEvent<bool>();

    private void Update()
    {

        Vector3 direction = targetSun.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        Physics.Raycast(ray, out RaycastHit hitInfo, 1000);
        if (hitInfo.collider.gameObject == targetSun.gameObject)
        {
            output.Invoke(true);
            state = true;
        }
        else
        {
            output.Invoke(false);
            state = false;
        }

    }



}
