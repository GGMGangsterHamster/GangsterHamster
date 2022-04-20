using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Player.Move;

namespace Objects.StageObjects
{
   [RequireComponent(typeof(CollisionInteractableObject))]
   public class Treadmill : MonoBehaviour, ICollisionEventable
   {
      [field: SerializeField]
      public Vector3 PushDirection { get; set; }

      // 코루틴 돌리기 위한 boolean 저장 용
      private Dictionary<int, Coroutine> _coroutines;

      private void Awake()
      {
         _coroutines = new Dictionary<int, Coroutine>();
      }

      public void Active(GameObject other)
      {
         if(other.TryGetComponent<MoveDelta>(out var delta))
         {
            _coroutines.Add(other.GetInstanceID(),
                            StartCoroutine(Push(delta)));
         }
      }

      public void Deactive(GameObject other)
      {
         int id = other.GetInstanceID();

         if(_coroutines.ContainsKey(id))
         {
            StopCoroutine(_coroutines[id]);
            _coroutines.Remove(id);
         }
      }

      IEnumerator Push(MoveDelta target)
      {
         while(true)
         {
            target.AddRawDelta(PushDirection);
            yield return null;
         }
      }
      
   }
}