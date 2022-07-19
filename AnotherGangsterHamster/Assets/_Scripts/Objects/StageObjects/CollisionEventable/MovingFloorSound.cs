using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;
using Objects.StageObjects.CollisionEventable;

public class MovingFloorSound : SoundController
{
    private MovingFloor _movingFloor;

    void Start()
    {
        _movingFloor = GetComponent<MovingFloor>(); 
    }

    public override void PlaySound(object obj)
    {
        SoundManager.Instance.Play("ObjectMoving");
    }
}
