using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions.Broker;

public class GravitoAbilitySound : MonoBehaviour
{
    [SerializeField] private AudioSource gravitoAbilityAudio;
    [SerializeField] private GravitoMessageBroker gravitoMessageBroker;

    void Start()
    {
        gravitoMessageBroker.OnUse.AddListener(GravitoAbilitySound_Play);
    }

    public void GravitoAbilitySound_Play()
    {
        gravitoAbilityAudio.Play();
    }
}