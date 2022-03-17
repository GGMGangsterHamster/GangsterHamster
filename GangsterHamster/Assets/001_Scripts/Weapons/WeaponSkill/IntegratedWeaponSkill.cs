using Objects.Interactable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class IntegratedWeaponSkill : Singleton<IntegratedWeaponSkill>, ISingletonObject
{
    // 플레이어 앞에 무언가 상호작용 가능한 오브젝트가 있다면 false, 없으면 true 리턴
    public bool CheckForward(Vector3 boxCenterOffset = new Vector3())
    {
        Vector3 boxSize = PlayerTrm.GetComponent<BoxCollider>().size;

        Collider[] cols = Physics.OverlapBox(PlayerTrm.position + PlayerTrm.up * 1.2f + boxCenterOffset,
                                             boxSize + new Vector3(0, -1.5f, 1f),
                                             PlayerTrm.rotation); // 플레이어의 바로 앞을 검사해서 뭔가 있는지 확인

        for (int i = 0; i < cols.Length; i++)
        {
            if(cols[i].TryGetComponent(out WeaponSkill skill) || cols[i].transform == PlayerTrm || cols[i].isTrigger)
            {
                continue;
            }
            Debug.Log(cols[i].name);
            if (cols[i].TryGetComponent(out Interactable outII)) // 만약 상호작용 되는 오브젝트가 앞에 있었으면 리턴
            {
                // 뭔가에 막힌다 그럼 여기로 옴
                return false;
            }
        }

        return true;
    }


}
