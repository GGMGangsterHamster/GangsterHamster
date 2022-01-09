using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static private T _instance = null;
    static public T Instance {
        get {
            if(_instance == null) {
                T[] objs = FindObjectsOfType<T>();
                if(objs.Length > 0) {
                    _instance = objs[0];
                    if (objs.Length > 1) {
                        // err
                    }
                    DontDestroyOnLoad(_instance.gameObject);
                } else {
                    // err
                }

            }

            return _instance;
        }
    }
}
