using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingComponent : BaseLogicComponent
{
    private void Awake()
    {
        inputs.Add(new LogicInput<bool>().inputDelegate += (bool b) => Invert(b));
        outputs.Add(new LogicOutput<bool>());
    }

    private void Invert(bool b)
    {
        (outputs[0] as LogicOutput<bool>).outputDelegate.Invoke(!b);
    }
}
