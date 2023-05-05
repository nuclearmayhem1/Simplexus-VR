using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseLogicComponent : MonoBehaviour
{
    public List<object> inputs = new List<object>();
    public List<object> outputs = new List<object>();
}
