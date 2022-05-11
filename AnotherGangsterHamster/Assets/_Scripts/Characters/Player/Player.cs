using UnityEngine;
using UnityEngine.Events;

namespace Characters.Player
{

   public class Player : CharacterBase
   {
      [SerializeField]
      private Transform _deadCallbacks = null;

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
         Debug.Log("�׾����!");
         PlayerStatus.Moveable = false;
         // �״´ٸ� ������ �ؾ� �ұ�?

         // ���: 
         Utils.ExecuteCallback(_deadCallbacks);         
      }

      protected override void Awake()
      {
         base.Awake();
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