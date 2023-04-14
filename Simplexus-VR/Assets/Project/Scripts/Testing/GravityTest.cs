using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class GravityTest : MonoBehaviour
{
    public Transform targetBody;
    public float gravity;
    public Vector3 movementVector = Vector3.zero;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 gravityUp = (targetBody.position - transform.position).normalized;



        rb.AddForce(gravityUp * gravity * Time.deltaTime);
        rb.AddRelativeForce(movementVector * Time.deltaTime);

    }


}
