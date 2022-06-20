using UnityEngine;

static partial class Utils
{
   static public Vector3 Multiply(this Vector3 one, Vector3 two)
      => new Vector3(one.x * two.x, one.y * two.y, one.z * two.z);
}