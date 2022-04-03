using UnityEngine;


namespace Physics.Gravity
{
   public class GravityManager : MonoBehaviour
   {
      static private GravityManager _instance;

      private WaitForEndOfFrame _wait;
      private GravityValue _globalGravity;

      private GravityAffectedObject[] _affectedObjects;

      private bool _stop = true;
      public bool Stop
      {
         get => _stop;
         set => _stop = value;
      }

      private void Awake()
      {
         _instance = this;

         _wait = new WaitForEndOfFrame();
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
         return _instance._globalGravity._direction;
      }
   }
}