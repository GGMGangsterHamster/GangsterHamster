using Characters.Player.Mouse;
using UnityEngine;

namespace Objects.Camrea
{
   public class CameraCalibration : MonoBehaviour
   {
      [Header("응시 거리")]
      [Range(1.0f, 5.0f)]
      public float focusDistance = 4.0f;

      const float DEFAULT_HEIGHT = 1.657952f;

      private GameObject _playerBase;
      public GameObject PlayerBase
      {
         get
         {
            if (_playerBase == null)
               _playerBase = GameObject.FindWithTag("PLAYER_BASE");

            return _playerBase;
         }
      }

      private Transform _headTrm = null;
      private Transform HeadTrm
      {
         get
         {
            if (_headTrm == null)
               _headTrm = GameObject.FindWithTag("PLAYER_HEAD").transform;

            return _headTrm;
         }
      }

      private Mouse _mouse;

      private void Start()
      {
         _mouse = FindObjectOfType<Mouse>();
      }

      private void LateUpdate()
      {
         float y        = _mouse.transform.localEulerAngles.y * Mathf.Deg2Rad;
         float sinZ     = Mathf.Sin(_mouse.rotY * Mathf.Deg2Rad);
         float absSinZ  = Mathf.Abs(sinZ);
         float sinY     = Mathf.Sin(y);
         float cosY     = Mathf.Cos(y);

         Vector3 target
            = new Vector3(sinY - absSinZ * sinY, sinZ, cosY - absSinZ * cosY);
         target *= focusDistance;
         target.y += DEFAULT_HEIGHT;

         // Vector3 add = HeadTrm.localPosition;
         // add.y = 0.0f;

         target += HeadTrm.position;
         transform.LookAt(target, PlayerBase.transform.up);
      }
   }
}