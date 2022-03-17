using Objects.Interactable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class IntegratedWeaponSkill : Singleton<IntegratedWeaponSkill>, ISingletonObject
{
    // �÷��̾� �տ� ���� ��ȣ�ۿ� ������ ������Ʈ�� �ִٸ� false, ������ true ����
    public bool CheckForward(Vector3 boxCenterOffset = new Vector3())
    {
        Vector3 boxSize = PlayerTrm.GetComponent<BoxCollider>().size;

        Collider[] cols = Physics.OverlapBox(PlayerTrm.position + PlayerTrm.up * 1.2f + boxCenterOffset,
                                             boxSize + new Vector3(0, -1.5f, 1f),
                                             PlayerTrm.rotation); // �÷��̾��� �ٷ� ���� �˻��ؼ� ���� �ִ��� Ȯ��

        for (int i = 0; i < cols.Length; i++)
        {
            if(cols[i].TryGetComponent(out WeaponSkill skill) || cols[i].transform == PlayerTrm || cols[i].isTrigger)
            {
                continue;
            }
            Debug.Log(cols[i].name);
            if (cols[i].TryGetComponent(out Interactable outII)) // ���� ��ȣ�ۿ� �Ǵ� ������Ʈ�� �տ� �־����� ����
            {
                // ������ ������ �׷� ����� ��
                return false;
            }
        }

        return true;
    }


}
