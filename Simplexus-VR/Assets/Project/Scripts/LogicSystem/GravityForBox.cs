using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityForBox : MonoBehaviour
{
    public Transform center;
    public Rigidbody r;

    public float velocity = 10;

    private void Update()
    {
        r.AddForce((transform.position - center.position) * velocity * Time.deltaTime);
    }


}
