using Objects.InteractableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions;

public class ObjectColisionSound : MonoBehaviour
{
    [SerializeField] private AudioSource colisionAudio;
    [SerializeField] private CollisionInteractableObject collisionInteractableObject;
    [SerializeField] private Grand grand;

    void Start()
    {
        collisionInteractableObject.Callbacks[0].OnActive.AddListener(ObjectColisionSound_Play);
    }

    void ObjectColisionSound_Play(GameObject game)
    {
        if (!grand.IsHandleWeapon())
        {
            if (!colisionAudio.isPlaying)
            {
                colisionAudio.Play();
            }
        }
    }
}
