using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions.Broker;
using Sound;

public class LumoAbilitySound : MonoBehaviour
{
    [SerializeField] private AudioSource lumoAbilityAudio;
    [SerializeField] private LumoMessageBroker lumoMessageBroker;

    void Start()
    {
        SoundManager.Instance.AddAudioSource(lumoAbilityAudio.clip.name, lumoAbilityAudio);
        lumoMessageBroker.OnUse.AddListener(LumoAbilitySound_Play);
    }

    public void LumoAbilitySound_Play()
    {
        lumoAbilityAudio.Play();
    }
}