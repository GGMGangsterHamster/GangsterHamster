using Objects.InteractableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions;

public class ObjectCollisionSound : MonoBehaviour
{
    [SerializeField] private AudioSource objCollisionAudio;
    [SerializeField] private CollisionInteractableObject collisionInteractableObject;
    [SerializeField] private Grand grand;

    void Start()
    {
        collisionInteractableObject.Callbacks[0].OnActive.AddListener(ObjectCollisionSound_Play);
    }

    public void ObjectCollisionSound_Play(GameObject obj)
    {
        if (!grand.IsHandleWeapon())
        {
            objCollisionAudio.Play();
        }
    }
}
