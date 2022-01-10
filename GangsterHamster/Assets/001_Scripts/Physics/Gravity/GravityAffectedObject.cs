using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gravity.Object
{
    /// <summary>
    /// 중력 영향을 받는 오브젝트가 가지고 있어야 하는 컴포넌트
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class GravityAffectedObject : MonoBehaviour, IGravityAffected
    {
        public int ID { get; set; } = -1; // 식별용
        public bool AffectedByGlobalGravity { get; set; } = true; // 전역 중력 영향 받는 여부

        private Rigidbody rigid;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }
        
        public virtual void Gravity(Vector3 dir, float amount)
        {
            rigid.AddForce(dir * amount, ForceMode.Force);
        }
    }
}