using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogHelper : MonoBehaviour
{
    public static void logError(string klassenname, string methodenname, string fehlermeldung)
    {
        Debug.LogError(">>> " + klassenname + " - " + methodenname + " <<< Error: " + fehlermeldung);
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
    public static void DebugMe(int i)
    {
        Debug.Log("## Drin ## -> " + i);
    }
    public static void DebugMe(bool myBool)
    {
        Debug.Log("## Drin ## -> " + myBool.ToString());
    }

}
