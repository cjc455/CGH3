using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonoSingleton
{
    static Dictionary<string, GameObject> singletons = null;
    
    static private void Initialize()
    {
        singletons = new Dictionary<string, GameObject>();
    }
    public static GameObject GetSingleton(string singletonName)
    {
        if(singletons == null)
        {
            Initialize();
        }
        GameObject testObject;
        singletons.TryGetValue(singletonName, out testObject);
        if(testObject == null)
        {
            testObject = GameObject.Find(singletonName);
            singletons.Add(singletonName, testObject);
        }
        return testObject;
    }
    public static bool DestroySingleton(string singletonName)
    {
        if (singletons == null)
        {
            Initialize();
        }
        GameObject testObject;
        singletons.TryGetValue(singletonName, out testObject);
        if (testObject == null)
        {
            return false;
        }
        Object.Destroy(testObject);
        return true;
    }
    /*
    public static GameObject InstansiateSingleton(string newSingletonName)
    {
        GameObject newObj = new GameObject();
        newObj.name = newSingletonName;
        return newObj;
    }
    */
    
}