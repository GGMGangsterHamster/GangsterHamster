using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    // �̳ʽÿ�, �׷���, �׷����� 3������ ������� �����ϱ� ���� �θ� Ŭ�����̸�
    // ����� ��ɵ��� �Լ��� ���� �س��⵵ �Ͽ���.
    public class WeaponAction : MonoBehaviour
    {
        [HideInInspector] public WeaponEnum _weaponEnum; // ��ӹ��� ������ ����

        public bool possibleUse = false; // ����� �����Ѱ�

        // �⺻���� �Լ���

        /// <summary>
        /// ��Ŭ������ ���� �߻�
        /// </summary>
        public virtual void FireWeapon()
        {

        }

        /// <summary>
        /// ��Ŭ������ �ɷ� �ߵ�
        /// </summary>
        public virtual void UseWeapon()
        {

        }

        /// <summary>
        /// R�� ���� ȸ��
        /// </summary>
        public virtual void ResetWeapon()
        {

        }

        /// <summary>
        /// �׳� ���� �÷��̾ �ش��ϴ� ������Ʈ�� ��� �ֳ��� �˻��ϴ� �Լ�
        /// </summary>
        /// <returns></returns>
        public virtual bool IsHandleWeapon()
        {
            return false;
        }

        /// <summary>
        /// ���� ���ڰ��� ���� SetActive True, false ���ִ� �Լ�
        /// </summary>
        public bool SetActiveWeaponObj(WeaponEnum wenum)
        {
            if (!possibleUse) return false;

            gameObject.SetActive(wenum == _weaponEnum || !IsHandleWeapon());

            return gameObject.activeSelf;
        }


    }
}