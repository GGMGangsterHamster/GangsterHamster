using UnityEngine;

static public partial class Utils
{
   static public bool Compare(Vector3 one, Vector3 two, float threshold = 0.08f)
   {
      return (one.x <= two.x + threshold && one.x >= two.x - threshold) &&
             (one.y <= two.y + threshold && one.y >= two.y - threshold) &&
             (one.z <= two.z + threshold && one.z >= two.z - threshold);
   }
}