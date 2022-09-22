using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Weapons.Actions.Broker;
using UnityEngine.Rendering;
using System;

public class CameraReBoundBlur : MonoBehaviour
{
    [SerializeField] private VolumeProfile mainCamVolume;

    [Header("FocusDistance 변수")]
    [SerializeField] private float minTargetBlurRadius;
    [SerializeField] private float maxTargetBlurRadius;

    [Header("Duration 변수")]
    [SerializeField] private float blurDuration;
    [SerializeField] private float onBlurLerpDuration;
    [SerializeField] private float offBlurLerpDuration;

    private DepthOfField depthOfField;
    private GrandMessageBroker grandMessageBroker;
    private Coroutine blurCoroutine;

    void Start()
    {
        mainCamVolume.TryGet<DepthOfField>(out depthOfField);
        grandMessageBroker = FindObjectOfType<GrandMessageBroker>();
        grandMessageBroker.OnRebound.AddListener(ReBoundBlur);
    }

    IEnumerator LerpBlur()
    {
        float elapseTime = 0f;

        while (elapseTime < onBlurLerpDuration)
        {
            elapseTime += Time.deltaTime;
            depthOfField.farMaxBlur = Mathf.Lerp(depthOfField.farMaxBlur, maxTargetBlurRadius, elapseTime);
            yield return null;
        }

        elapseTime = 0f;
        yield return new WaitForSeconds(blurDuration);

        while (elapseTime < offBlurLerpDuration)
        {
            elapseTime += Time.deltaTime;
            depthOfField.farMaxBlur = Mathf.Lerp(depthOfField.farMaxBlur, minTargetBlurRadius, elapseTime);
            yield return null;
        }
    }

    public void ReBoundBlur()
    {
        StartCoroutine(LerpBlur());
    }
}
