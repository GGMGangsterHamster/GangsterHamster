using System;
using UnityEngine;

namespace Obejcts.Damageable
{
    // 생각을 조금 더 해보고 작성해야겟슴
    abstract public class DamageableObject<T> : MonoBehaviour, IDamageable<T>
    {
        abstract public T HP { get; set; }

        public virtual void OnDamage(T damage)
        {
            
        }

        public void OnDead(Action callback = null)
        {
            
        }
    }

}