using Gravity.Object;
using Objects.Interactable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NormalObjA : ObjectA
{
    private void Start()
    {
        gameObject.AddComponent<GravityAffectedObject>();
    }
    public override void Interact(Action callback = null)
    {
        callback?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transform.root == null) return;

        if(transform.root.TryGetComponent(out FirstWeaponSkill outSkill) && collision.transform.CompareTag("PLAYER_BASE"))
        {
            Vector3 moveDir = (MainCamTrm.position - transform.position).normalized;
            transform.position += moveDir * Time.deltaTime * outSkill.shotSpeed;

            PlayerBaseTrm.GetComponent<Rigidbody>().velocity = moveDir * outSkill.knockbackPower * outSkill.comeBackTime;

            outSkill.comeBackTime = 0f;
            outSkill.ComeBack();
        }
    }

    public override void Release()
    {
        if(transform.TryGetComponent(out SecondWeaponSkill skill))
        {
            skill.ComeBack();
        }
        else
        {
            base.Release();
        }
    }
}
