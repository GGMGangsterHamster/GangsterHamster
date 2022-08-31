using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;
using Objects.StageObjects.CollisionEventable;

public class BrakeGlassSound : SoundController
{
    private Glass glass;

    void Start()
    {
        glass = GetComponent<Glass>();
        glass.OnBreak.AddListener(BrakeGlass);
    }

    void BrakeGlass()
    {
        SoundManager.Instance.Play("GlassBrake");
    }

    public override void PlaySound(object obj)
    {
       
    }
}
