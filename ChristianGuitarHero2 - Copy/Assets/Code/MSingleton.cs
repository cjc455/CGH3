using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MSingleton {

    static List<Component> quickAccessSingletons;
    static private bool initialized = false;
    static bool debugMode = false;
    public static t GetSingleton<t>() where t : Component
    {
        CheckInit();
        t returnT;
        returnT = (t)quickAccessSingletons.Find(s => typeof(t) == s.GetType());
        if(returnT == null)
        {
            t[] objects = GameObject.FindObjectsOfType<t>();
            if(objects.Length > 1)
            {
                Debug.LogError("Found multiple instances of singleton " + typeof(t).ToString());
                
            }
            else if (objects.Length == 0)
            {
                Debug.LogError("Found 0 instances of singleton " + typeof(t).ToString());
            }
            returnT = objects[0];
            if(debugMode)
                Debug.Log("found new singleton " + returnT.name);
        }
        if (debugMode)
            Debug.Log("returning " + returnT.name);
        return returnT;
    }

    private static void CheckInit()
    {
        if(!initialized)
        {
            quickAccessSingletons = new List<Component>();
        }
    }
}
