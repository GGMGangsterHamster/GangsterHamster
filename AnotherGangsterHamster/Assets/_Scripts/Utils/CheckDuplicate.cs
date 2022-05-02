using UnityEngine;

static public partial class Utils
{
    /// <summary>
    /// 중복 체크를 함
    /// </summary>
    /// <returns>중복 시 true</returns>
    static public bool CheckDuplicate<T>() where T : Component
    {
        var objs = MonoBehaviour.FindObjectsOfType<T>();

        return objs.Length > 1;
    }
}