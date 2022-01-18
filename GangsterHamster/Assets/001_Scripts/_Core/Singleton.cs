using System;

/// <summary>
/// Mono 상속받지 않은 클래스용 Singleton
/// </summary>
/// <typeparam name="T"></typeparam>
abstract public class Singleton<T> where T : class, ISingletonObject
{
    private T _instance = null;
    public T Instance {
        get {
            if (_instance == null) {
                _instance = Activator.CreateInstance<T>();
                GC.KeepAlive(_instance);
            }

            return _instance;
        }
    }
}
