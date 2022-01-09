using System;
using UnityEngine;

[Serializable]
abstract public class JsonObject
{
    /// <summary>
    /// JSON 데이터로 클래스 멤버 변수들을 오버라이드함
    /// </summary>
    /// <param name="json">JSON 문자열</param>
    public virtual void OverrideData(string json)
    {
        try
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{this.GetType()}: Cannot Load Settings. Reverting to default.\r\n{e}");
        }
    }

    /// <summary>
    /// JSON 으로 변환됨
    /// </summary>
    /// <returns>this as Json string</returns>
    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}