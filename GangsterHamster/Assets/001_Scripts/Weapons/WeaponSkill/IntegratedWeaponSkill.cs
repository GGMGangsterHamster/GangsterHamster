using Objects.Interactable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class IntegratedWeaponSkill : Singleton<IntegratedWeaponSkill>, ISingletonObject
{
    /// <summary>
    /// �÷��̾� �տ� ���� ��ȣ�ۿ� ������ ������Ʈ�� �ִٸ� false, ������ true ����
    /// </summary>
    /// <param name="boxCenterOffset">üũ�� ��ġ�� offset ��Ű�� ��</param>
    /// <returns></returns>
    public bool CheckForward(Vector3 boxCenterOffset = new Vector3())
    {
        Collider[] cols = Physics.OverlapBox(PlayerBaseTrm.position + PlayerBaseTrm.up * 1.2f + boxCenterOffset,
                                             new Vector3(0.5f, 0.3f, 0.65f),
                                             PlayerBaseTrm.rotation); // �÷��̾��� �ٷ� ���� �˻��ؼ� ���� �ִ��� Ȯ��

        for (int i = 0; i < cols.Length; i++)
        {
            if(cols[i].TryGetComponent(out WeaponSkill skill) || cols[i].transform == PlayerBaseTrm || cols[i].isTrigger)
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
