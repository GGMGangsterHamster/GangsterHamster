using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.InteractableObjects;
using Sound;

public class PressSwitchSound : MonoBehaviour
{
    [SerializeField] private AudioSource buttonAudio;
    [SerializeField] private BothInteractableObject bothInteractableObject;
    private bool isPressed = false;

    void Start()
    {
        SoundManager.Instance.AddAudioSource(buttonAudio.clip.name, buttonAudio);
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
