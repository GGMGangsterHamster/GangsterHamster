using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    // �̳ʽÿ�, �׷���, �׷����� 3������ ������� �����ϱ� ���� �θ� Ŭ�����̸�
    // ����� ��ɵ��� �Լ��� ���� �س��⵵ �Ͽ���.
    public class WeaponAction : MonoBehaviour
    {
        public WeaponEnum _weaponEnum; // ��ӹ��� ������ ����

        public bool possibleUse = false; // ����� �����Ѱ�

        // �⺻���� �Լ���
        public virtual void ShotWeapon()
        {
            // 1. ��Ŭ������ ���� �߻�
        }

        public virtual void ActivateWeapon()
        {
            // 2. ��Ŭ������ �ɷ� �ߵ�
        }

        public virtual void ResetWeapon()
        {
            // 3. R�� ���� ȸ��
        }
    }
}