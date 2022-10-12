using Objects.StageObjects.CollisionEventable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;

public class ObjectMovingSound : MonoBehaviour
{
    [SerializeField] private AudioSource objectMovingSound;

    void Start()
    {
        SoundManager.Instance.AddAudioSource(objectMovingSound.clip.name, objectMovingSound);
    }
}
