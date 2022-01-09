using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggerTest : MonoBehaviour
{
    private void Start() {
        Logger.Logger.Log("Nothing special");
        Logger.Logger.Log("Just a warning.", Logger.LogLevel.Warning);
        Logger.Logger.Log("errr...", Logger.LogLevel.Error);
        Logger.Logger.Log("no", Logger.LogLevel.Fatal);
    }
}
