using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions.Broker;

public class GrandAbilitySound : MonoBehaviour
{
    [SerializeField] private AudioSource grandAbilityAudio;
    [SerializeField] private GrandMessageBroker grandMessageBroker;

    void Start()
    {
        grandMessageBroker.OnUse.AddListener(GrandAbilitySound_Play);
    }

    public void GrandAbilitySound_Play(int num)
    {
        grandAbilityAudio.Play();
    }
}
