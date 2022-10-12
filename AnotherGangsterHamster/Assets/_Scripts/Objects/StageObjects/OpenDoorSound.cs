using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;

public class OpenDoorSound : SoundController
{
    public void OpenDoorPlaySound()
    {
        SoundManager.Instance.Play("OpenDoor");
    }

    public override void PlaySound(object obj)
    {
       
    }
}
