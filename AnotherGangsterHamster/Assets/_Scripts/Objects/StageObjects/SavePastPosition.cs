using UnityEngine;

namespace Objects.StageObjects
{
   public class SavePastPosition
   {
      private Vector3 _pastPos;

      public Vector3 GetPastPos()
         => _pastPos;

      public void SetPastPos(Vector3 value)
         => _pastPos = value;

      public Vector3 CalculateDelta(Vector3 curPos)
         => _pastPos - curPos;
   }
}