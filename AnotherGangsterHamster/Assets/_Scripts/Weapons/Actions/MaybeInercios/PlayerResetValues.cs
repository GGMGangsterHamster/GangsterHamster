using Characters.Player;
using Matters.Gravity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
   public class PlayerResetValues : MonoBehaviour
   {
      private GravityAffectedObject _playerGravity;

      private void Awake()
      {
         _playerGravity = GameObject.FindGameObjectWithTag("PLAYER_BASE").GetComponent<GravityAffectedObject>();
      }

      public void Execute()
      {
         if (_playerGravity.AffectedByGlobalGravity)
         {
            PlayerValues.Speed = PlayerValues.DashSpeed;
         }
      }
   }
}
