using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;

public class GravitoAbilitySound : SoundController
{
    public override void PlaySound(object obj)
    {
        SoundManager.Instance.Play("GravitoAbilityExecute");
    }
}
