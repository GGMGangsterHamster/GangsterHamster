using UnityEngine;

namespace _Core.Initialize.InitScripts
{
   public class AttachCamToPlayer : InitBase
   {
      // 카메라가 고정될 위치
      private Transform _playerCamLockTrm = null;
      private Transform PlayerCamLockTrm
      {
         get
         {
            if (_playerCamLockTrm == null)
            {
               _playerCamLockTrm = GameObject
                        .FindWithTag("PLAYER_CAMERA_LOCK")
                        .transform;
            }

            return _playerCamLockTrm;
         }
      }

      private Transform _mainCam;
      private Transform MainCam
      {
         get
         {
            if (_mainCam == null)
               _mainCam = Camera.main.transform;

            return _mainCam;
         }
      }

      public override RunLevel RunLevel => RunLevel.SCENE_LOAD;

      public override void Call()
      {
         MainCam.SetParent(PlayerCamLockTrm);
         MainCam.transform.localPosition = Vector3.zero;
      }
   }
}