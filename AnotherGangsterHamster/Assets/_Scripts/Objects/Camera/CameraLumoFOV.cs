using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions.Broker;
using Objects.InteractableObjects;
using Weapons.Actions;

public class CameraLumoFOV : MonoBehaviour
{
    [SerializeField] private float lumoFOV;
    [SerializeField] private float onLerpDuration;
    [SerializeField] private float offLerpDuration;

    private Camera mainCam;
    private float defaultFOV;
    private Coroutine currnetCoroutine;

    void Start()
    {
        mainCam = FindObjectOfType<Camera>();

        defaultFOV = mainCam.fieldOfView;
    }

    IEnumerator LerpFOV(float start, float end, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            mainCam.fieldOfView = Mathf.Lerp(start, end, timeElapsed);
            yield return null;  
        }
    }

    public void OnLumoFOV()
    {
        if (currnetCoroutine != null)
        {
            StopCoroutine(currnetCoroutine);
        }
        
        currnetCoroutine = StartCoroutine(LerpFOV(mainCam.fieldOfView, lumoFOV, onLerpDuration));
    }

    public void OffLumoFOV()
    {
        if (currnetCoroutine != null)
        {
            StopCoroutine(currnetCoroutine);
        }

        currnetCoroutine = StartCoroutine(LerpFOV(mainCam.fieldOfView, defaultFOV, offLerpDuration));
    }
}
