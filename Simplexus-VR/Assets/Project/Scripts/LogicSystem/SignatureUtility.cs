using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public static class SignatureUtility
{

    public static bool CompareSignatures(MethodInfo method1, MethodInfo method2)
    {
        if (method1.ReturnType != method2.ReturnType)
        {
            return false;
        }

        var parameters1 = method1.GetParameters();
        var parameters2 = method2.GetParameters();

        if (parameters1.Length != parameters2.Length)
        {
            return false;
        }

        for (int i = 0; i < parameters1.Length; i++)
        {
            if (parameters1[i].ParameterType != parameters2[i].ParameterType)
            {
                return false;
            }
        }

        return true;
    }

}
