using System.Collections;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using Objects.InteractableObjects;
using Objects.StageObjects.CollisionEventable;

public class BrakeGlassSound : MonoBehaviour
{
    [SerializeField] private AudioSource brakeAudio;
    [SerializeField] private CollisionInteractableObject collisionInteractableObject;
    [SerializeField] private Glass glass;
         
    void Start()
    {
        SoundManager.Instance.AddAudioSource(brakeAudio.clip.name, brakeAudio);
        glass.OnBreak.AddListener(BrakeGlassSound_Play);
    }

    public void BrakeGlassSound_Play()
    {
        Debug.Log("깨진 유리창의 법칙");
        brakeAudio.Play();  
    }
}
