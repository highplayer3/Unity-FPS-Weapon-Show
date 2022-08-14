using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Singleton<T> where T : new()
{
    private static T instance;
    private static object mutex = new object();

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (mutex)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }
}

public class UnitySingleton<T> : MonoBehaviour where T : Component
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = (T)obj.AddComponent(typeof(T));
                    obj.hideFlags = HideFlags.DontSave;
                    obj.name = typeof(T).Name;
                }
            }
            return instance;
        }
    }
    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}


