using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;

public class CollisionSound : SoundController
{
    public override void PlaySound(object obj)
    {
        SoundManager.Instance.Play("Audio/SoundEffect/ObjectCollision");
    }
}
