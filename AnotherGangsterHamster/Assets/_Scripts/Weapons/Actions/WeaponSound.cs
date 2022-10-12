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
    public void WeaponGrebSound()
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
