using Characters.Player;
using Matters.Gravity;
using Objects.StageObjects.CollisionEventable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeGlassSound : MonoBehaviour
{
    [SerializeField] private AudioSource brakeGlassAudio;
    [SerializeField] private Glass glass;

    public void BrakeGlassSound_Play()
    {
        brakeGlassAudio.Play();
    }
}
