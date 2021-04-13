using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{

    public static void CheckIfPlayerDataIsNull(object checkerObject)
    {
        if (checkerObject != null)
        {
            Debug.Log("NIX NULL");
        }
        else
        {
            Debug.Log("NULL!!!!!!!!!!!!!");
        }
    }
    public static void CheckIfPlayerDataIsNull(object checkerObject, string gimmeAString)
    {
        if (checkerObject != null)
        {
            Debug.Log(gimmeAString + ": NIX NULL");
        }
        else
        {
            Debug.Log(gimmeAString + ": NULL!!!!!!!!!!!!!");
        }
    }

    public static void DebugMe()
    {
        Debug.Log("## Drin ##");
    }
    public static void DebugMe(float y)
    {
        Debug.Log("## Drin ## -> " + y.ToString());
    }
    public static void DebugMe(string v)
    {
        Debug.Log("## Drin ## -> " + v);
    }

}
