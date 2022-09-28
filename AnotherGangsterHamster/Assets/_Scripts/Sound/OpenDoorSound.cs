using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorSound : MonoBehaviour
{
    [SerializeField] private AudioSource openDoorAudio;

    public void OpenDoorSound_Play()
    {
        openDoorAudio.Play();
    }
}
