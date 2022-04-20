using UnityEngine;


namespace Physics.Gravity
{
   public class GravityManager : MonoBehaviour
   {
      static private GravityManager _instance; // static 함수 용

      private GravityValue             _globalGravity;   // 전역 중력
      private GravityAffectedObject[]  _affectedObjects; // 중력 영향 받는 오브젝트들

      // 중력 연산 여부
#region bool Stop
      private  bool _stop = true;
      public   bool Stop
      {
         get => _stop;
         set => _stop = value;
      }
#endregion // bool Stop

      private void Awake()
      {
         _instance      = this;
         _globalGravity = new GravityValue(Vector3.down);

         InitGravityAffectedObjects();

         Stop = false;
      }

      private void FixedUpdate()
      {
         if(Stop) return;

         foreach(GravityAffectedObject obj in _affectedObjects)
         {
            if(!obj.AffectedByGlobalGravity) continue;

            obj.Gravity(_globalGravity);
         }

      }

      /// <summary>
      /// 중력 영향을 받는 오브젝트를 찾고, 초기화 합니다.
      /// </summary>
      private void InitGravityAffectedObjects()
      {
         _affectedObjects =
            FindObjectsOfType<GravityAffectedObject>();

         for (int i = 0; i < _affectedObjects.Length; ++i)
         {
            _affectedObjects[i]
               .GetComponent<Rigidbody>().useGravity = false;
         }
      }

      static public void ChangeGlobalGravityDirection(Vector3 direction)
      {
         _instance._globalGravity._direction = direction;
      }     

      static public void ChangeGlobalGravityForce(float force)
      {
         _instance._globalGravity._force = force;
      }

      static public Vector3 GetGlobalGravityDirection()
      {
         return _instance._globalGravity._direction.normalized;
      }
   }
}