using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AlignToParent : MonoBehaviour
{
#if UNITY_EDITOR

    private void Update()
    {
        Vector3 upDirection = (transform.position - transform.parent.position).normalized;
        transform.rotation = Quaternion.FromToRotation(transform.up, upDirection) * transform.rotation;
    }
#endif
}
