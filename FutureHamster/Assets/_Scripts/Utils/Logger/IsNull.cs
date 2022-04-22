using System;

static class NULL
{
    /// <summary>
    /// Null checker for your convenience<br/>
    /// Only checks inside unity editor
    /// </summary>
    /// <param name="obj">Object that you want to check if it is null</param>
    /// <param name="whenNullAction">called when null<br/>Leaves error log when null</param>
    /// <param name="whenNotNullAction">called when not null</param>
    static public void Check(object obj, Action whenNullAction = null, Action whenNotNullAction = null)
    {
        if(obj == null) {
            Logger.Log($"Type: handled object is null.", LogLevel.Error);
            whenNullAction?.Invoke();
        } else {
            whenNotNullAction?.Invoke();
        }
    }
}