using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;
using Characters.Player.Actions;
using Weapons.Actions;
using UnityEngine.Events;

public class WeaponSound : SoundController
{
    private ActionInputHandler _actionInputHandler;
    private WeaponManagement _weaponManagement;

    void Start()
    {
        _weaponManagement = FindObjectOfType<WeaponManagement>();
        _actionInputHandler = GetComponent<ActionInputHandler>();
        _actionInputHandler.interaction.Execute.AddListener(WeaponGrebSound);
    }

    public void WeaponGrebSound(object obj)
    {
        SoundManager.Instance.Play("Audio/SoundEffect/WeaponDraw");
    }

    public void WeaponFireSound(object obj)
    {
        SoundManager.Instance.Play("Audio/SoundEffect/WeaponFire");
    }

    public override void PlaySound(object obj)
    {
        
    }
}
