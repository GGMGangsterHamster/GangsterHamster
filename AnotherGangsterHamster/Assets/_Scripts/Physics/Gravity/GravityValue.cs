using UnityEngine;

namespace Physics.Gravity
{
   /// <summary>
   /// 중력 값 저장용 클래스
   /// </summary>
   public class GravityValue
   {
      public Vector3 _direction;

      public float _force;

      public GravityValue(Vector3 direction, float force = 9.8f)
      {
         this._direction = direction;
         this._force = force;
      }

   }

}