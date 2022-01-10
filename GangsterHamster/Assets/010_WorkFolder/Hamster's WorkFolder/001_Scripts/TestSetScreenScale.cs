using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSetScreenScale : MonoBehaviour
{
    public int width = 1920;
    public int height = 1080;
    public FullScreenMode fullScreenModeEnum = FullScreenMode.Windowed;
    public int preferredRefreshRate = 1;

    private void Update()
    {
        Screen.SetResolution(width, height, fullScreenModeEnum, preferredRefreshRate);
    }
}
