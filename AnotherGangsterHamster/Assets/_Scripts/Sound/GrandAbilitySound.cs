using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions.Broker;

public class GrandAbilitySound : MonoBehaviour
{
    [SerializeField] private GrandMessageBroker messageBroker;
    [SerializeField] private AudioSource grandAudio;

    void Start()
    {
        messageBroker.OnUse.AddListener(GrandAbilitySound_Play);
    }

    void GrandAbilitySound_Play(int num)
    {
        grandAudio.Play();
    }
}
