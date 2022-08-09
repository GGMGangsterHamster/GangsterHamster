using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;
using Objects.StageObjects.CollisionEventable;

public class MovingFloorSound : SoundController
{
    public void MovingFloorPlaySound()
    {
        SoundManager.Instance.Play("ObjectMoving");
    }

    public override void PlaySound(object obj)
    {
        
    }
}
