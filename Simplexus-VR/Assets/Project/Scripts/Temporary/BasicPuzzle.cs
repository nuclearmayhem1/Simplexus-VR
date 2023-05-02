using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPuzzle : MonoBehaviour
{
    public Animator door;
    public Solarpanel solarpanel;
    public ToggleInteractable lever;

    private void Update()
    {
        if (lever.state == true && solarpanel.state == true)
        {
            door.Play("Open_Door");
        }
        else
        {
            door.Play("Close_Door");
        }
    }

}
