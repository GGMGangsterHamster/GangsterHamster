using Player.Status;
using UnityEngine;


namespace Player.Utils
{
   public class PlayerUtils : Singleton<PlayerUtils>, ISingletonObject
   {
      private Transform _playerTrm = null;
      private Transform PlayerTrm
      {
         get
         {
            if(_playerTrm == null)
            {
               _playerTrm = GameObject.FindGameObjectWithTag("PLAYER").transform;
            }
            
            return _playerTrm;
         }
      }

      private CapsuleCollider _collider = null;
      private CapsuleCollider Collider
      {
         get
         {
            if(_collider == null)
            {
               _collider = GameManager.Instance.Player.GetComponent<CapsuleCollider>();
            }

            return _collider;
         }
      }

      /// <summary>
      /// 웅크린 상태로 변경
      /// </summary>
      public void SetCrouched()
      {
         PlayerStatus.IsCrouching = true;
         PlayerValues.speed = PlayerValues.CrouchSpeed;

         PlayerTrm.localScale = new Vector3(PlayerTrm.localScale.x,
                                             PlayerValues.PlayerCrouchYScale,
                                             PlayerTrm.localScale.z);

         PlayerTrm.localPosition = new Vector3(0.0f,
                                                PlayerValues.PlayerCrouchYPos,
                                                0.0f);

         Collider.height = 1.0f;
         Collider.center = new Vector3(0.0f, 0.5f, 0.0f);

         SetWalking();
      }

      /// <summary>
      /// 일어선 상태로 변경
      /// </summary>
      public void SetStanded()
      {
         PlayerStatus.IsCrouching = false;
         PlayerValues.speed = PlayerValues.WalkingSpeed;

         PlayerTrm.localScale = new Vector3(PlayerTrm.localScale.x,
                                             PlayerValues.PlayerYScale,
                                             PlayerTrm.localScale.z);

         PlayerTrm.localPosition = new Vector3(0.0f,
                                                PlayerValues.PlayerYPos,
                                                0.0f);

         Collider.height = 1.8f;
         Collider.center = new Vector3(0.0f, 0.9f, 0.0f);
      }

      public void SetRunning()
      {
         PlayerStatus.IsRunning = true;
         PlayerValues.speed = PlayerValues.DashSpeed;
         PlayerValues.headBobAmplitude = PlayerValues.RunHeadBobAmplitude;
         PlayerValues.headBobFrequency = PlayerValues.RunHeadBobFrequency;
      }

      public void SetWalking()
      {
         PlayerStatus.IsRunning = false;
         PlayerValues.speed = PlayerValues.WalkingSpeed;
         PlayerValues.headBobAmplitude = PlayerValues.WalkHeadBobAmplitude;
         PlayerValues.headBobFrequency = PlayerValues.WalkHeadBobFrequency;
      }


   }
}