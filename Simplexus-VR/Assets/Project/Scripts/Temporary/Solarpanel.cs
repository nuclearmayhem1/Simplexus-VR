using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solarpanel : MonoBehaviour
{
    public bool state = false;
    public Transform targetSun;


    private void Update()
    {

        Vector3 direction = targetSun.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        Physics.Raycast(ray, out RaycastHit hitInfo, 1000);
        if (hitInfo.collider.gameObject == targetSun.gameObject)
        {
            state = true;
        }
        else
        {
            state = false;
        }

    }



}
