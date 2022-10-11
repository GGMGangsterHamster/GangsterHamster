using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.InteractableObjects;

public class PressSwitchSound : MonoBehaviour
{
    [SerializeField] private AudioSource buttonAudio;
    [SerializeField] private BothInteractableObject bothInteractableObject;
    private bool isPressed = false;

    void Start()
    {
        bothInteractableObject.Callbacks[0].OnDeactive.AddListener(NonPressSwitch);
    }

    public void PressSwitchSound_Play()
    {
        if (!isPressed)
        {
            buttonAudio.Play();
            isPressed = true;
        }
    }

    public void NonPressSwitch(GameObject game)
    {
        isPressed = false;
    }
}
