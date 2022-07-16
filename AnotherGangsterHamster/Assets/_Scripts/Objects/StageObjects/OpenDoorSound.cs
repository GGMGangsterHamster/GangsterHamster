using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;

public class OpenDoorSound : SoundController
{
    public override void PlaySound(object obj)
    {
        SoundManager.Instance.Play("Audio/SoundEffect/OpenDoor");
    }
}
