using Objects.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

/// <summary>
/// 2��° ������ Ÿ���� A��� ���� �����ؼ� 
/// ����� �� ��ũ��Ʈ
/// </summary>
public class WeaponObjA : ObjectA
{
    SecondWeaponSkill skill;

    private void Start()
    {
        skill = GetComponent<SecondWeaponSkill>();
    }
    public override void Release()
    {
        if(transform.TryGetComponent(out SecondWeaponSkill ii))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            transform.parent = null;
            ii.gameObject.SetActive(false);
            
        }
    }

    public void FirstWeaponCollision()
    {
        skill.StopShotingMove();
    }
}
