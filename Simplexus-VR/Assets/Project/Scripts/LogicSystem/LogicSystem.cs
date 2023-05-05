using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class LogicSystem
{
    public delegate void LogicDelegate<T>(T value);
}

public class LogicInput<T>
{
    public LogicSystem.LogicDelegate<T> inputDelegate;
    public string Name;
    public string Description;

    public LogicInput(string name, string description)
    {
        inputDelegate = null;
        Name = name;
        Description = description;
    }

    public LogicInput(string name)
    {
        inputDelegate = null;
        Name = name;
        Description = "A input value of the type: " + typeof(T).FullName;
    }

    public LogicInput()
    {
        inputDelegate = null;
        Name = "input " + typeof(T).FullName;
        Description = "A input value of the type: " + typeof(T).FullName;
    }
}

public class LogicOutput<T>
{
    public LogicSystem.LogicDelegate<T> outputDelegate;
    public string Name;
    public string Description;

    public LogicOutput(string name, string description)
    {
        outputDelegate = null;
        Name = name;
        Description = description;
    }

    public LogicOutput(string name)
    {
        outputDelegate = null;
        Name = name;
        Description = "A output value of the type: " + typeof(T).FullName;
    }

    public LogicOutput()
    {
        outputDelegate = null;
        Name = "output " + typeof(T).FullName;
        Description = "A output value of the type: " + typeof(T).FullName;
    }
}