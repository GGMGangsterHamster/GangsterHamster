using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions.Broker;
using Sound;

public class GravitoAbilitySound : MonoBehaviour
{
    [SerializeField] private AudioSource weaponFireAudio;
    [SerializeField] private AudioSource weaponDrawAudio;
    [SerializeField] private AudioSource gravitoAbilityAudio;
    [SerializeField] private GravitoMessageBroker gravitoMessageBroker;

    void Start()
    {
        SoundManager.Instance.AddAudioSource(gravitoAbilityAudio.clip.name, gravitoAbilityAudio);
        gravitoMessageBroker.OnFire.AddListener(WeaponFireSound_Play);
        gravitoMessageBroker.OnUse.AddListener(GravitoAbilitySound_Play);
        gravitoMessageBroker.OnReset.AddListener(GravitoAbilitySound_Stop);
        gravitoMessageBroker.OnReset.AddListener(WeaponDrawSound_Play);
    }

    public void WeaponFireSound_Play()
    {
        weaponFireAudio.Play();
    }

    public void WeaponDrawSound_Play()
    {
        weaponDrawAudio.Play();
    }

    public void GravitoAbilitySound_Play()
    {
        gravitoAbilityAudio.Play();
    }

    public void GravitoAbilitySound_Stop()
    {
        gravitoAbilityAudio.Stop();
    }
}