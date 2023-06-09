using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pressureplate_Component : MonoBehaviour
{
    public UnityEvent<bool> onSwitch = new UnityEvent<bool>();
    public UnityEvent off = new UnityEvent();
    public UnityEvent on = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onSwitch.Invoke(true);
            on.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onSwitch.Invoke(false);
            on.Invoke();
        }
    }

}
