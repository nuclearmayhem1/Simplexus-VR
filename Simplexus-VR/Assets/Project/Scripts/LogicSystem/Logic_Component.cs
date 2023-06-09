using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Logic_Component : MonoBehaviour
{
    private bool b1 , b2, b3, b4;

    public Operation operation;

    public void In1(bool b)
    {
        b1 = b;
        Process();
    }
    public void In2(bool b)
    {
        b2 = b;
        Process();
    }


    public UnityEvent<bool> output = new UnityEvent<bool>();


    private void Process()
    {
        switch (operation)
        {
            case Operation.not:
                output.Invoke(!b1);
                break;
            case Operation.and:
                output.Invoke(b1 && b2);
                break;
            case Operation.or:
                output.Invoke(b1 || b2);
                break;
            default:
                break;
        }
    }





}

public enum Operation
{
    not, and, or
}