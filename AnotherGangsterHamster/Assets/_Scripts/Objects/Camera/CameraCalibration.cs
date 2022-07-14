using Characters.Player.Mouse;
using UnityEngine;

namespace Objects.Camrea
{
   public class CameraCalibration : MonoBehaviour
   {
      [Header("응시 거리")]
      [Range(1.0f, 5.0f)]
      public float focusDistance = 4.0f;

      const float DEFAULT_HEIGHT = 1.66118f;

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
         float y = PlayerBase.transform.localEulerAngles.y  * Mathf.Deg2Rad;
         float z = HeadTrm.transform.localEulerAngles.z     * Mathf.Deg2Rad;
         float sinZ = Mathf.Sin(z);

         Vector3 target
            = new Vector3(Mathf.Sin(y), sinZ, Mathf.Cos(y)).normalized;
         Debug.Log(target);
         target *= focusDistance;
         target.y += DEFAULT_HEIGHT;

         target += PlayerBase.transform.position;


         transform.LookAt(target, PlayerBase.transform.up);

         // FIXME: 완벽하게 아레를 보고 있지 않음

         Debug.DrawLine(transform.position, target, Color.red);
      }
   }
}