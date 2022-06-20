using UnityEngine;

static public partial class Utils
{
   static public Vector3 Abs(this Vector3 one)
   {
      return new Vector3(Mathf.Abs(one.x),
                         Mathf.Abs(one.y),
                         Mathf.Abs(one.z));
   }
}