using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameChecker : MonoBehaviour
{
    public Text frameText;

    float secondChecker = 0f;
    int framePerSecond = 0;

    void Update()
    {
        if(secondChecker >= 1f)
        {
            frameText.text = "Update Frame : " + framePerSecond.ToString();
            secondChecker = 0f;
            framePerSecond = 0;
        }
        else
        {
            ++framePerSecond;
        }

        secondChecker += Time.deltaTime;
    }
}
