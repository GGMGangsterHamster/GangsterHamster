using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions.Broker;
using Sound;

public class LumoAbilitySound : MonoBehaviour
{
    [SerializeField] private AudioSource lumoAbilityAudio;
    [SerializeField] private AudioSource weaponFireAudio;
    [SerializeField] private AudioSource weaponDrawAudio;
    [SerializeField] private LumoMessageBroker lumoMessageBroker;

    void Start()
    {
        SoundManager.Instance.AddAudioSource(lumoAbilityAudio.clip.name, lumoAbilityAudio);
        lumoMessageBroker.OnUse.AddListener(LumoAbilitySound_Play);
        lumoMessageBroker.OnFire.AddListener(WeaponFireSound_Play);
        lumoMessageBroker.OnReset.AddListener(WeaponDrawSound_Play);
        lumoMessageBroker.OnReset.AddListener(LumoAbilitySound_Stop);
    }

    public void LumoAbilitySound_Play()
    {
        lumoAbilityAudio.Play();
    }

    public void LumoAbilitySound_Stop()
    {
        lumoAbilityAudio.Stop();
    }

    public void WeaponFireSound_Play()
    {
        weaponFireAudio.Play();
    }

    public void WeaponDrawSound_Play()
    {
        weaponDrawAudio.Play();
    }
}