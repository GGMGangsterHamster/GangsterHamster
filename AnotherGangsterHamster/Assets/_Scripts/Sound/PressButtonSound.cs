using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;
using Objects.InteractableObjects;

public class PressButtonSound : MonoBehaviour 
{
    [SerializeField] private AudioSource buttonAudio;
    [SerializeField] private TriggerInteractableObject triggerInteractableObject;
    private bool isPressed = false;

    void Start()
    {
        triggerInteractableObject.Callbacks[0].OnDeactive.AddListener(NonPressButton);
    }

    public void PressButtonSound_Play()
    {
        if (!isPressed)
        {
            buttonAudio.Play();
            isPressed = true;
        }
    }

    public void NonPressButton(GameObject game)
    {
        isPressed = false;
    }
}
