using UnityEngine;
using UnityEngine.Events;

namespace Characters.Player
{

   public class Player : CharacterBase
   {
      [Header("Invoked when dead")]
      public UnityEvent OnDead = new UnityEvent();

      public float regenerationDelay;
      public int regenerationValue;
      private float _currentRegenerationTime;

      public override void Damage(int damage)
      {
         _hp -= damage;
         _currentRegenerationTime = regenerationDelay;

         if (_hp <= 0)
         {
            Dead();
         }
      }

      protected override void Dead()
      {
         if (_isDead) return;

         Debug.Log("�׾����!");
         _isDead = true;
         PlayerStatus.Moveable   = false;
         PlayerStatus.Jumpable   = false;
         PlayerStatus.Crouchable = false;
         // �״´ٸ� ������ �ؾ� �ұ�?

         // ���: 
         OnDead.Invoke();
      }

      protected override void Awake()
      {
         base.Awake();
      }

      public void SetMaxHP(int hp)
      {
          _maxHp = hp;
          _hp = hp;
      }

      private void Update()
      {
         if (_currentRegenerationTime <= 0)
         {
            if (_hp < _maxHp)
            {
               _hp += regenerationValue;
            }
         }
         else
         {
            _currentRegenerationTime -= Time.deltaTime;
         }
      }
   }
}