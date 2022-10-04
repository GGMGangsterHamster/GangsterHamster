using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Weapons.Actions.Broker;
using UnityEngine.Rendering;
using System;
using Weapons.Actions;
using _Core.Initialize;

public class CameraReBoundBlur : MonoBehaviour
{
    [SerializeField] private VolumeProfile mainCamVolume;

    [Header("FocusDistance ����")]
    [SerializeField] private float minTargetBlurRadius;
    [SerializeField] private float maxTargetBlurRadius;

    [Header("Duration ����")]
    [SerializeField] private float blurDuration;
    [SerializeField] private float onBlurLerpDuration;
    [SerializeField] private float offBlurLerpDuration;

    private DepthOfField depthOfField;
    private GrandMessageBroker grandMessageBroker;
    private OpenRC openRC;
    private IEnumerator coroutine;

    private bool _enabled = true;
    private Action onResetBlur;

    void Start()
    {
        if (FindObjectOfType<WeaponManagement>()?.startHandleWeapon != WeaponEnum.Grand)
        {
            enabled = false;
            _enabled = false;
            return;
        }

        mainCamVolume.TryGet<DepthOfField>(out depthOfField);
        grandMessageBroker = FindObjectOfType<GrandMessageBroker>();
        grandMessageBroker.OnRebound.AddListener(ReBoundBlur);
        onResetBlur = ResetBlur;
        openRC = FindObjectOfType<OpenRC>();
        openRC.Add(RunLevel.SCENE_LOAD, onResetBlur);
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
        depthOfField.farMaxBlur = maxTargetBlurRadius;

        elapseTime = 0f;
        yield return new WaitForSeconds(blurDuration);

        while (elapseTime < offBlurLerpDuration)
        {
            elapseTime += Time.deltaTime;
            depthOfField.farMaxBlur = Mathf.Lerp(depthOfField.farMaxBlur, minTargetBlurRadius, elapseTime);
            yield return null;
        }
        depthOfField.farMaxBlur = minTargetBlurRadius;
    }

    public void ReBoundBlur()
    {
        if (!_enabled) return;

        StartCoroutine(LerpBlur());
    }

    void ResetBlur()
    {
        depthOfField.farMaxBlur = minTargetBlurRadius;
    }
}
