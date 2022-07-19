using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;
using Characters.Player.Actions;
using Weapons.Actions;
using UnityEngine.Events;
using Weapons.Actions.Broker;

public class WeaponSound : SoundController
{
    private ActionInputHandler _actionInputHandler;
    private AllWeaponMessageBroker _allWeaponMessageBroker;
    private GrandMessageBroker _grandMessageBroker;
    private GravitoMessageBroker _gravitoMessageBroker;
    private LumoMessageBroker _lumoMessageBroker;

    void Start()
    {
        _actionInputHandler =      GetComponent<ActionInputHandler>();
        _allWeaponMessageBroker =  FindObjectOfType<AllWeaponMessageBroker>();
        _grandMessageBroker =      FindObjectOfType<GrandMessageBroker>();
        _gravitoMessageBroker =    FindObjectOfType<GravitoMessageBroker>();
        _lumoMessageBroker =       FindObjectOfType<LumoMessageBroker>();

        _actionInputHandler.interaction.Execute.AddListener(WeaponGrebSound);
        _allWeaponMessageBroker.OnFire.AddListener(WeaponFireSound);
        _grandMessageBroker.OnUse.AddListener(GrandAbilitySound);
        _gravitoMessageBroker.OnUse.AddListener(GravitoAbilitySound);
        _lumoMessageBroker.OnUse.AddListener(LumoAbilitySound);
    }

    public void WeaponGrebSound(object obj)
    {
        SoundManager.Instance.Play("WeaponDraw");
    }

    public void WeaponFireSound()
    {
        SoundManager.Instance.Play("WeaponFire");
    }

    public void GrandAbilitySound()
    {
        SoundManager.Instance.Play("GrandAbilityExecute");
    }

    public void GravitoAbilitySound()
    {
        SoundManager.Instance.Play("GravitoAbilityExecute");
    }

    public void LumoAbilitySound()
    {
        SoundManager.Instance.Play("LumoAbilityContinue");
    }

    public override void PlaySound(object obj)
    {
        
    }
}
