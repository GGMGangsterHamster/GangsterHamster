using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions;

namespace Characters.Player
{
    public class PlayerPressureCheck : MonoBehaviour
    {
        public float pressureDistance;
        Player player;
        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void Update()
        {
            if (IsPressure())
                player.Damage(1000);
        }
        private bool IsPressure()
        {
            float distance = 0f;

            bool forward = Physics.Raycast(transform.position + transform.up / 4, transform.forward, out RaycastHit forwardHit);
            bool back = Physics.Raycast(transform.position + transform.up / 4, -transform.forward, out RaycastHit backHit);
            bool right = Physics.Raycast(transform.position + transform.up / 4, transform.right, out RaycastHit rightHit);
            bool left = Physics.Raycast(transform.position + transform.up / 4, -transform.right, out RaycastHit leftHit);

            bool grandCheck = false;

            if(forward && back)
            {
                distance = Vector3.Distance(forwardHit.point, backHit.point);

                if(forwardHit.transform.TryGetComponent(out Grand g1) || backHit.transform.TryGetComponent(out Grand g2))
                {
                    grandCheck = true;
                }
            }
            if(right && left)
            {
                distance = (Vector3.Distance(rightHit.point, leftHit.point) < distance) ? Vector3.Distance(rightHit.point, leftHit.point) : distance;
            
                if(rightHit.transform.TryGetComponent(out Grand g3) || leftHit.transform.TryGetComponent(out Grand g4))
                {
                    grandCheck = true;
                }
            }

            if (distance == 0 || !grandCheck) return false; // �� �ٱ��� �´�� �ִٸ� Ȯ�� �� �� �� �׷��尡 ���ٸ�

            return distance < pressureDistance;
        }
    }
}