using Characters.Damage;
using UnityEngine;

namespace Characters
{
   abstract public class CharacterBase : MonoBehaviour, IDamageable
   {
      [SerializeField] protected int _maxHp = 100;
      abstract public void Damage(int damage);
      abstract protected void Dead();
      protected bool _isDead;
      protected int _hp;

      protected virtual void Awake()
      {
         _hp = _maxHp;
         _isDead = false;
      }
   }
}