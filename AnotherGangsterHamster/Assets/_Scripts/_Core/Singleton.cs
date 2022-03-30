using System;

/// <summary>
/// Mono 상속받지 않은 클래스용 Singleton
/// </summary>
abstract public class Singleton<T> where T : class
{
    static private T _instance = null;
    static public T Instance {
        get {
            if (_instance == null) {
                _instance = Activator.CreateInstance<T>();
                GC.KeepAlive(_instance);
            }

            return _instance;
        }
    }
}
