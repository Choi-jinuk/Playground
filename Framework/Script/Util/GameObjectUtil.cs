using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtil
{
    public static T AddComponent<T>(GameObject go, bool bActive = true) where T : UnityEngine.Component
    {
        T scriptObject = go.GetComponent<T>();
        if (scriptObject == null)
        {
            scriptObject = go.AddComponent<T>();
            if (scriptObject == null)
            {
                DebugManager.LogError($"Failed AddComponent ({go.name})");
                return null;
            }
        }
        scriptObject.gameObject.SetActive(bActive);
        return scriptObject;
    }

    public static T FindComponent<T>(GameObject parent, string name, ref T component, bool bAddComponent = false) where T : UnityEngine.Component
    {
        if (component == null)
        {
            GameObject go = FindChild(parent, name);
            if (go == null)
            {
                return null;
            }

            component = go.GetComponent<T>();
            if (bAddComponent == false)
            {
                if (component == null)
                    return null;
            }
            else if (component == null)
            {
                component = go.AddComponent<T>();
            }
        }

        return component;
    }

    public static GameObject FindChild(GameObject parent, string childName)
    {
        if (parent == null)
        {
            DebugManager.LogError($"FindChild() parent is Null Child name is {childName}");
            return null;
        }

        if (parent.name == childName)
        {
            return parent;
        }

        foreach (Transform child in parent.transform)
        {
            if (child == null)
                continue;

            GameObject go = FindChild(child.gameObject, childName);
            if (go != null)
                return go;
        }

        return null;
    }

    public static T FindChild<T>(GameObject parent, string childName) where T : Object
    {
        if (parent == null)
        {
            DebugManager.LogError($"FindChild() parent is Null Child name is {childName}");
            return null;
        }
        
        if (parent.name == childName)
        {
            var component = parent.GetComponent<T>();
            if (component != null)
                return component;
        }
        
        foreach (Transform child in parent.transform)
        {
            if (child == null)
                continue;

            var component = FindChild<T>(child.gameObject, childName);
            if (component != null)
                return component;
        }

        return null;
    }

    public static T FindComponentInChild<T>(GameObject parent, ref T component) where T : UnityEngine.Component
    {
        component = parent.GetComponent<T>();
        if (component != null)
            return component;

        foreach (Transform child in parent.transform)
        {
            component = child.GetComponent<T>();
            if (component == null)
            {
                FindComponentInChild<T>(child.gameObject, ref component);
            }
            else
            {
                if (component != null)
                    return component;
            }
        }

        return null;
    }
    public static void FindComponentsInChild<T>(GameObject parent, ref List<T> components) where T : UnityEngine.Component
    {
        T com = parent.GetComponent<T>();
        if(com != null)
            components.Add(com);

        foreach (Transform child in parent.transform)
        {
            com = child.GetComponent<T>();
            if (com == null)
            {
                FindComponentsInChild<T>(child.gameObject, ref components);
            }
            else
            {
                components.Add(com);
            }
        }
    }

    public static void Destroy(GameObject go)
    {
        if (go == null)
            return;
        
        List<Renderer> list = new List<Renderer>();
        FindComponentsInChild(go, ref list);

        var count = list.Count;
        for (int i = 0; i < count; i++)
        {
            Object.Destroy(list[i].material);
        }
        Object.Destroy(go);
    }
}
