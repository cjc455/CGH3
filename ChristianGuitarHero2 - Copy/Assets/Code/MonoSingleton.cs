using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonoSingleton
{
    Dictionary<GameObject, string> singletons;
    
    private void Initialize()
    {
        singletons = new Dictionary<GameObject, string>();
    }
    public static GameObject GetSingleton(string singletonName)
    {
        return GameObject.Find(singletonName);
    }
    public static bool DestroySingleton(string singletonName)
    {
        GameObject obj = GameObject.Find(singletonName);
        if(obj)
        {
            Object.Destroy(obj);
            return true;
        }
        return false;
    }
    public static GameObject InstansiateSingleton(string newSingletonName)
    {
        GameObject newObj = new GameObject();
        newObj.name = newSingletonName;
        return newObj;
    }
    
}