using Player.Status;
using UnityEngine;


namespace Player.Utils
{
   public class PlayerUtils : Singleton<PlayerUtils>, ISingletonObject
   {
      private Transform _playerTrm;
      private CapsuleCollider _collider;



      public PlayerUtils()
      {
         _playerTrm = GameObject.FindGameObjectWithTag("PLAYER").transform;
         _collider = GameManager.Instance.Player.GetComponent<CapsuleCollider>();
      }

      /// <summary>
      /// 웅크린 상태로 변경
      /// </summary>
      public void SetCrouched()
      {
         PlayerStatus.IsCrouching = true;
         PlayerValues.speed = PlayerValues.CrouchSpeed;

         _playerTrm.localScale = new Vector3(_playerTrm.localScale.x,
                                             PlayerValues.PlayerCrouchYScale,
                                             _playerTrm.localScale.z);

         _playerTrm.localPosition = new Vector3(0.0f,
                                                PlayerValues.PlayerCrouchYPos,
                                                0.0f);

         _collider.height = 1.0f;
         _collider.center = new Vector3(0.0f, 0.5f, 0.0f);

         SetWalking();
      }

      /// <summary>
      /// 일어선 상태로 변경
      /// </summary>
      public void SetStanded()
      {
         PlayerStatus.IsCrouching = false;
         PlayerValues.speed = PlayerValues.WalkingSpeed;

         _playerTrm.localScale = new Vector3(_playerTrm.localScale.x,
                                             PlayerValues.PlayerYScale,
                                             _playerTrm.localScale.z);

         _playerTrm.localPosition = new Vector3(0.0f,
                                                PlayerValues.PlayerYPos,
                                                0.0f);

         _collider.height = 1.8f;
         _collider.center = new Vector3(0.0f, 0.9f, 0.0f);
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