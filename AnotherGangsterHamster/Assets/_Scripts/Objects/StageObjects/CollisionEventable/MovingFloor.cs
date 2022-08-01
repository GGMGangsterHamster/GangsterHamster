using UnityEngine;
using Tween;


namespace Objects.StageObjects.CollisionEventable
{
   public class MovingFloor : MonoBehaviour, IEventable
   {
      public   Vector3  target      = Vector3.zero;
      public   float    duration    = 2.0f;
      public   float    threshould  = 0.08f;
      public   bool     startActive = false;
      private  Vector3  _initalPos  = Vector3.zero; // 기본 포지션

      private Coroutine _up = null;
      private Coroutine _down = null;

      private void Awake()
      {
         _initalPos = this.transform.localPosition;

         if(startActive)
         {
            Active(null);
         }
      }

      public void Active(GameObject other)
      {
         if (_down != null || _up != null)
            ValueTween.Stop(this);

         Vector3 step = target / duration;
         Vector3 final = _initalPos + target;

         
         _up = ValueTween.To(this,
                  () => {
                     transform.localPosition += step * Time.deltaTime;
                  },
                  () => {
                     return Utils.Compare(transform.localPosition, final, threshould);
                  },
                  () => {
                     transform.localPosition = final;
                     _up = null;
                  });
      }

      public void Deactive(GameObject other)
      {
         if(_down != null || _up != null)
            ValueTween.Stop(this);

         Vector3 step = target / duration;

         _down = ValueTween.To(this,
                  () => {
                     transform.localPosition -= step * Time.deltaTime;
                  },
                  () => {
                     return Utils.Compare(transform.localPosition, _initalPos, threshould);
                  },
                  () => {
                     transform.localPosition = _initalPos;
                     _down = null;
                  });
      }
   }
}