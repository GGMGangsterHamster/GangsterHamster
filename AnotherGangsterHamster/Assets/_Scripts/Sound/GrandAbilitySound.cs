using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions.Broker;
using Sound;

public class GrandAbilitySound : MonoBehaviour
{
    [SerializeField] private GrandMessageBroker messageBroker;
    [SerializeField] private AudioSource grandAudio;
    [SerializeField] private AudioSource weaponDrawAudio;

    void Start()
    {
        SoundManager.Instance.AddAudioSource(grandAudio.clip.name, grandAudio);
        messageBroker.OnUse.AddListener(GrandAbilitySound_Play);
        messageBroker.OnReset.AddListener(WeaponDrawSound_Play);
    }

    void GrandAbilitySound_Play(int num)
    {
        grandAudio.Play();
    }

    void WeaponDrawSound_Play()
    {
        weaponDrawAudio.Play();
    }
}
