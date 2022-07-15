using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;

public class MovingFloorSound : SoundController
{
    public override void PlaySound(object obj)
    {
        SoundManager.Instance.Play("Audio/SoundEffect/ObjectMoving");
    }
}
