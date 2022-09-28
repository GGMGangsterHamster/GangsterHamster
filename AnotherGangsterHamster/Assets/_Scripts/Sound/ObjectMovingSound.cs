using Objects.StageObjects.CollisionEventable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovingSound : MonoBehaviour
{
    [SerializeField] private AudioSource objectMovingSound;
    [SerializeField] private MovingFloor movingFloor;

    void Start()
    {
        movingFloor.OnEventActive.AddListener(ObjectMovingSound_Play);
        movingFloor.OnEventDeactive.AddListener(ObjectMovingSound_Stop);
    }

    public void ObjectMovingSound_Play(bool enabled)
    {
        objectMovingSound.Play();
    }

    public void ObjectMovingSound_Stop(bool enabled)
    {
        objectMovingSound.Stop();
    }
}
