using Characters.Damage;
using UnityEngine;

namespace Characters
{
   abstract public class CharacterBase : MonoBehaviour, IDamageable
   {
      [SerializeField] protected int _maxHp = 100;
                       protected int _hp;
      abstract         public    void Damage(int damage);

      protected virtual void Awake()
      {
         _hp = _maxHp;
      }
   }
}