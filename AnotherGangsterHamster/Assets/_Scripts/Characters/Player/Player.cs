using UnityEngine;

namespace Characters.Player
{
   
   public class Player : CharacterBase
   {
      

      public override void Damage(int damage)
      {
         _hp -= damage;
         // code...
      }
   }
}