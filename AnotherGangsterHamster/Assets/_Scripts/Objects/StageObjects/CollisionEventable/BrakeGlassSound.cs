using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;
using Objects;

public class BrakeGlassSound : SoundController, IEventable
{
    public void Active(GameObject other)
    {
        SoundManager.Instance.Play("GlassBrake");
    }

    public void Deactive(GameObject other)
    {
    }

    public override void PlaySound(object obj)
    {
        // throw new System.NotImplementedException();
    }
}
